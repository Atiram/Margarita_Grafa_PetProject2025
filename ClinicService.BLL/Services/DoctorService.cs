using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using AutoMapper;
using Clinic.Domain;
using ClinicService.BLL.Models;
using ClinicService.BLL.Models.Requests;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;
using ClinicService.DAL.Utilities.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ClinicService.BLL.Services;
public class DoctorService(IDoctorRepository doctorRepository,
    IMapper mapper,
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory,
    ILogger<DoctorService> logger
    ) : IDoctorService
{
    private readonly int maxRetries = 3;
    private readonly TimeSpan delay = TimeSpan.FromSeconds(2);
    private const string FileServiceSectionName = "FileServiceBaseUrl";
    private readonly string fileServiceBaseUrl = configuration.GetSection(FileServiceSectionName).Value ??
        throw new ArgumentException(string.Format(NotificationMessages.SectionMissingErrorMessage, FileServiceSectionName));
    private HttpClient httpClient = httpClientFactory.CreateClient("FileService");

    public async Task<DoctorModel?> GetById(Guid id, CancellationToken cancellationToken)
    {
        var doctorEntity = await doctorRepository.GetByIdAsync(id, cancellationToken);
        if (doctorEntity != null)
        {
            var fileUrl = await GetPhotoAsync(doctorEntity.Id, cancellationToken);
            var doctorModel = mapper.Map<DoctorModel>(doctorEntity);

            return doctorModel;
        }
        logger.LogError(string.Format(NotificationMessages.NotFoundErrorMessage, id));
        return null;
    }

    public async Task<PagedResult<DoctorModel>> GetAll(GetAllDoctorsParams getAllDoctorsParams, CancellationToken cancellationToken)
    {
        var doctorEntities = await doctorRepository.GetAllAsync(getAllDoctorsParams, cancellationToken);
        return mapper.Map<PagedResult<DoctorModel>>(doctorEntities);
    }

    public async Task<DoctorModel> CreateAsync(CreateDoctorRequest request, CancellationToken cancellationToken)
    {
        var doctorEntity = await doctorRepository.CreateAsync(mapper.Map<DoctorEntity>(request), cancellationToken);
        await UploadPhotoAsync(doctorEntity.Id, request.Formfile, cancellationToken);
        return mapper.Map<DoctorModel>(doctorEntity);
    }

    public async Task<DoctorModel> UpdateAsync(UpdateDoctorRequest request, CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByIdAsync(request.Id, cancellationToken);
        if (doctor == null)
        {
            logger.LogError(string.Format(NotificationMessages.NotFoundErrorMessage, request.Id));
            throw new Exception(string.Format(NotificationMessages.NotFoundErrorMessage, request.Id));
        }

        var doctorEntity = mapper.Map(request, doctor);
        if (request.Formfile != null && request.Formfile.Length != 0)
        {
            await DeletePhotoAsync(doctorEntity.Id, cancellationToken);
            await UploadPhotoAsync(doctorEntity.Id, request.Formfile, cancellationToken);
        }
        var updatedDoctorEntity = await doctorRepository.UpdateAsync(doctorEntity, cancellationToken);

        return mapper.Map<DoctorModel>(updatedDoctorEntity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var doctorEntity = await doctorRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new Exception(string.Format(NotificationMessages.NotFoundErrorMessage, id));

        await DeletePhotoAsync(doctorEntity.Id, cancellationToken);
        var doctorDeleted = await doctorRepository.DeleteAsync(id, cancellationToken);
        return doctorDeleted;
    }

    private async Task<string> GetPhotoAsync(Guid doctorId, CancellationToken cancellationToken)
    {
        var getFileByReferenceIdEndpoint = $"{fileServiceBaseUrl}/referenceId/{doctorId}";
        var response = await ExecuteHttpRequestWithRetryAsync(
            () => httpClient.GetAsync(getFileByReferenceIdEndpoint, cancellationToken),
            nameof(GetPhotoAsync),
            doctorId);
        return await response.Content.ReadAsStringAsync();
    }

    private async Task UploadPhotoAsync(Guid doctorId, IFormFile? photoFile, CancellationToken cancellationToken)
    {
        if (photoFile == null || photoFile.Length == 0)
        {
            logger.LogWarning(string.Format(NotificationMessages.NotFoundErrorMessage, photoFile?.Name));
            throw new ValidationException(string.Format(NotificationMessages.NotFoundErrorMessage, photoFile?.Name));
        }

        using var content = new MultipartFormDataContent();
        using var streamContent = new StreamContent(photoFile.OpenReadStream());
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(photoFile.ContentType);
        content.Add(streamContent, "file", photoFile.FileName);
        content.Add(new StringContent(doctorId.ToString()), "referenceItemId");
        content.Add(new StringContent("Photo"), "documentType");
        content.Add(new StringContent($"{doctorId}.jpg"), "blobName");

        await ExecuteHttpRequestWithRetryAsync(
          () => httpClient.PostAsync(fileServiceBaseUrl, content, cancellationToken),
          nameof(UploadPhotoAsync),
          doctorId);
    }

    private async Task DeletePhotoAsync(Guid doctorId, CancellationToken cancellationToken)
    {
        var fileServiceDeleteEndpoint = $"{fileServiceBaseUrl}/referenceId22/{doctorId}";

        await ExecuteHttpRequestWithRetryAsync(
          () => httpClient.DeleteAsync(fileServiceDeleteEndpoint, cancellationToken),
          nameof(DeletePhotoAsync),
          doctorId);
    }

    private async Task<HttpResponseMessage> ExecuteHttpRequestWithRetryAsync(Func<Task<HttpResponseMessage>> httpRequest, string errorMessage, Guid? doctorId = null)
    {
        for (int i = 0; i <= maxRetries; i++)
        {
            try
            {
                var response = await httpRequest();
                response.EnsureSuccessStatusCode();
                return response;
            }
            catch (HttpRequestException ex) when (i < maxRetries)
            {
                logger.LogError(ex, string.Format(NotificationMessages.RetryingExecuteHttpRequestErrorMessage, i + 1, errorMessage, doctorId));
                await Task.Delay(delay);
            }
        }
        logger.LogError(string.Format(NotificationMessages.FailedExecuteHttpRequestErrorMessage, errorMessage, maxRetries));
        throw new InvalidOperationException(string.Format(NotificationMessages.FailedExecuteHttpRequestErrorMessage, errorMessage, maxRetries));
    }
}
