using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using Clinic.Domain;
using ClinicService.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ClinicService.BLL.Services;
public class DocumentService(
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory,
    ILogger<DoctorService> logger
    ) : IDocumentService
{
    private readonly int maxRetries = 3;
    private readonly TimeSpan delay = TimeSpan.FromSeconds(2);
    private const string FileServiceSectionName = "FileServiceBaseUrl";
    private readonly string fileServiceBaseUrl = configuration.GetSection(FileServiceSectionName).Value ??
        throw new ArgumentException(string.Format(NotificationMessages.SectionMissingErrorMessage, FileServiceSectionName));
    private HttpClient httpClient = httpClientFactory.CreateClient("FileService");

    public async Task<string> GetPhotoAsync(Guid doctorId, CancellationToken cancellationToken)
    {
        var getFileByReferenceIdEndpoint = $"{fileServiceBaseUrl}/referenceId/{doctorId}";
        var response = await ExecuteRequestWithRetryAsync(
            () => httpClient.GetAsync(getFileByReferenceIdEndpoint, cancellationToken),
            nameof(GetPhotoAsync),
            doctorId);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task UploadPhotoAsync(Guid doctorId, IFormFile? photoFile, CancellationToken cancellationToken)
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

        await ExecuteRequestWithRetryAsync(
          () => httpClient.PostAsync(fileServiceBaseUrl, content, cancellationToken),
          nameof(UploadPhotoAsync),
          doctorId);
    }

    public async Task DeletePhotoAsync(Guid doctorId, CancellationToken cancellationToken)
    {
        var fileServiceDeleteEndpoint = $"{fileServiceBaseUrl}/referenceId/{doctorId}";

        await ExecuteRequestWithRetryAsync(
          () => httpClient.DeleteAsync(fileServiceDeleteEndpoint, cancellationToken),
          nameof(DeletePhotoAsync),
          doctorId);
    }

    private async Task<HttpResponseMessage> ExecuteRequestWithRetryAsync(Func<Task<HttpResponseMessage>> httpRequest, string errorMessage, Guid? doctorId = null)
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
