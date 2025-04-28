using AutoMapper;
using Clinic.Domain;
using ClinicService.BLL.Models;
using ClinicService.BLL.Models.Requests;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;
using ClinicService.DAL.Utilities.Pagination;
using Microsoft.Extensions.Logging;

namespace ClinicService.BLL.Services;
public class DoctorService(IDoctorRepository doctorRepository,
    IDocumentService documentService,
    IMapper mapper,
    ILogger<DoctorService> logger
    ) : IDoctorService
{
    //private readonly int maxRetries = 3;
    //private readonly TimeSpan delay = TimeSpan.FromSeconds(2);
    //private const string FileServiceSectionName = "FileServiceBaseUrl";
    //private readonly string fileServiceBaseUrl = configuration.GetSection(FileServiceSectionName).Value ??
    //    throw new ArgumentException(string.Format(NotificationMessages.SectionMissingErrorMessage, FileServiceSectionName));
    //private HttpClient httpClient = httpClientFactory.CreateClient("FileService");

    public async Task<DoctorModel?> GetById(Guid id, CancellationToken cancellationToken)
    {
        var doctorEntity = await doctorRepository.GetByIdAsync(id, cancellationToken);
        if (doctorEntity != null)
        {
            var fileUrl = await documentService.GetPhotoAsync(doctorEntity.Id, cancellationToken);
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
        await documentService.UploadPhotoAsync(doctorEntity.Id, request.Formfile, cancellationToken);
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
            await documentService.DeletePhotoAsync(doctorEntity.Id, cancellationToken);
            await documentService.UploadPhotoAsync(doctorEntity.Id, request.Formfile, cancellationToken);
        }
        var updatedDoctorEntity = await doctorRepository.UpdateAsync(doctorEntity, cancellationToken);

        return mapper.Map<DoctorModel>(updatedDoctorEntity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var doctorEntity = await doctorRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new Exception(string.Format(NotificationMessages.NotFoundErrorMessage, id));

        await documentService.DeletePhotoAsync(doctorEntity.Id, cancellationToken);
        var doctorDeleted = await doctorRepository.DeleteAsync(id, cancellationToken);
        return doctorDeleted;
    }
}
