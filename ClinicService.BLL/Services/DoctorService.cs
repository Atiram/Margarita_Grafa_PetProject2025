using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;
using AutoMapper;
using Clinic.Domain;
using ClinicService.BLL.Models;
using ClinicService.BLL.Models.Requests;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.BLL.Utilities.Messages;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;
using ClinicService.DAL.Utilities.Pagination;
using Microsoft.Extensions.Configuration;

namespace ClinicService.BLL.Services;
public class DoctorService(IDoctorRepository doctorRepository, IMapper mapper, IConfiguration configuration) : IDoctorService //HttpClient httpClient, 
{
    private readonly string FileServiceSectionName = "FileServiceBaseUrl";
    private readonly string fileUrl = configuration.GetSection("FileServiceBaseUrl").Value ?? throw new ArgumentException("Section 'FileServiceBaseUrl' is missing or empty in configuration.");
    private HttpClient httpClient = new HttpClient();

    public async Task<DoctorModel> GetById(Guid id, CancellationToken cancellationToken)
    {
        var doctorEntity = await doctorRepository.GetByIdAsync(id, cancellationToken);

        return mapper.Map<DoctorModel>(doctorEntity);
    }

    public async Task<PagedResult<DoctorModel>> GetAll(GetAllDoctorsParams getAllDoctorsParams, CancellationToken cancellationToken)
    {
        var doctorEntities = await doctorRepository.GetAllAsync(getAllDoctorsParams, cancellationToken);
        return mapper.Map<PagedResult<DoctorModel>>(doctorEntities);
    }

    public async Task<DoctorModel> CreateAsync(CreateDoctorRequest request, CancellationToken cancellationToken)
    {
        if (request.FirstName.Length <= 3 || request.LastName.Length <= 3)
        {
            throw new ValidationException(ClinicNotificationMessages.validationExeptionMessage);
        }
        var doctorEntity = await doctorRepository.CreateAsync(mapper.Map<DoctorEntity>(request), cancellationToken);
        //IFormFile formFile = null;
        //string photoUrl 
        var fileModel = await UploadPhotoAsync(doctorEntity.Id, cancellationToken);
        var doctorModel = mapper.Map<DoctorModel>(doctorEntity);
        doctorModel.PhotoUrl = fileModel.StorageLocation;
        doctorModel.FileId = fileModel.Id;
        return doctorModel;
    }
    private async Task<FileModel> UploadPhotoAsync(Guid doctorId, CancellationToken cancellationToken) //IFormFile photoFile,
    {
        string fileServiceBaseUrl = fileUrl;
        string fileServiceUploadEndpoint = $"{fileServiceBaseUrl}?localFilePath=C:\\Users\\User\\Pictures\\Screenshots\\Screenshot 2025-04-09 174354.png";///upload/doctor/{doctorId}"; //fileServiceUploadEndpoint

        try
        {
            var createFileRequest = new CreateFileRequest
            {
                DocumentType = DocumentType.Photo,
                BlobName = $"{doctorId}.jpg",
                ReferenceItemId = doctorId.ToString()
            };

            HttpResponseMessage uploadResponse = await httpClient.PostAsJsonAsync(
                fileServiceUploadEndpoint,
                createFileRequest,
                cancellationToken);

            uploadResponse.EnsureSuccessStatusCode();
            //string responseBody = await uploadResponse.Content.ReadAsStringAsync();
            //using (JsonDocument jsonDocument = JsonDocument.Parse(responseBody))
            //{
            //    JsonElement root = jsonDocument.RootElement;
            //    if (root.TryGetProperty("storageLocation", out JsonElement storageLocationElement))
            //    {
            //        return storageLocationElement.GetString();
            //    }
            //    else
            //    {
            //        throw new InvalidOperationException("'storageLocation'not found");
            //    }
            //}

            var uploadedFile = await uploadResponse.Content.ReadFromJsonAsync<FileModel>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);
            if (uploadedFile != null)
            {
                return uploadedFile;//.StorageLocation;
            }
            else
            {
                throw new($"Failed to upload photo to DocumentService. Status code: {uploadResponse.StatusCode}");
            }

        }
        catch (HttpRequestException ex)
        {
            throw new($"Error communicating with DocumentService: {ex.Message}");
        }
        catch (JsonException ex)
        {
            throw new($"Error deserializing DocumentService response: {ex.Message}");
        }
    }

    public async Task<DoctorModel> UpdateAsync(UpdateDoctorRequest request, CancellationToken cancellationToken)
    {
        if (request.FirstName.Length <= 3 || request.LastName.Length <= 3)
        {
            throw new ValidationException(ClinicNotificationMessages.validationExeptionMessage);
        }
        var doctor = await doctorRepository.GetByIdAsync(request.Id, cancellationToken);
        var doctorEntity = mapper.Map(request, doctor);

        var updatedDoctorEntity = await doctorRepository.UpdateAsync(doctorEntity, cancellationToken);

        return mapper.Map<DoctorModel>(updatedDoctorEntity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            // 1. Retrieve Doctor Information
            var doctorEntity = await doctorRepository.GetByIdAsync(id, cancellationToken);
            if (doctorEntity == null)
            {
                return false; // Or throw an exception if you prefer
            }
            var doctorModel = mapper.Map<DoctorModel>(doctorEntity);

            string fileServiceBaseUrl = fileUrl;
            string fileServiceDeleteEndpoint = $"{fileServiceBaseUrl}/reference?referenceItemId={doctorModel.Id}&blobName={doctorModel.Id}.jpg";

            HttpResponseMessage deleteFileResponse = await httpClient.DeleteAsync(
                $"{fileServiceDeleteEndpoint}", cancellationToken);
            deleteFileResponse.EnsureSuccessStatusCode();
            bool doctorDeleted = await doctorRepository.DeleteAsync(id, cancellationToken);
            return doctorDeleted;
        }
        catch (HttpRequestException ex)
        {
            //_logger.LogError(ex, $"Error communicating with FileService: {ex.Message}");
            // Handle the exception, perhaps re-throw or return false
            return false; // Or throw an exception
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, $"Error deleting doctor: {ex.Message}");
            // Handle other exceptions
            return false; // Or throw an exception
        }
        //return await doctorRepository.DeleteAsync(id, cancellationToken);
    }
}
