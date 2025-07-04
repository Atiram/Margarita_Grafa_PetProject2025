using AutoMapper;
using Clinic.Domain;
using Clinic.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using OfficeService.BLL.Models;
using OfficeService.BLL.Models.Requests;
using OfficeService.BLL.Services.Interfaces;
using OfficeService.DAL.Entities;
using OfficeService.DAL.Repositories.Interfaces;

namespace OfficeService.BLL.Services;
public class OfficeService(IOfficeRepository officeRepository,
    IMapper mapper,
    ILogger<OfficeService> logger) : IOfficeService
{
    public async Task<OfficeModel> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var officeEntity = await officeRepository.GetByIdAsync(id, cancellationToken);
        if (officeEntity == null)
        {
            logger.LogWarning(NotificationMessages.NotFoundErrorMessage, id);
            throw new NotFoundException(string.Format(NotificationMessages.NotFoundErrorMessage, id));
        }
        return mapper.Map<OfficeModel>(officeEntity);
    }

    public async Task<List<OfficeModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var officeEntities = await officeRepository.GetAllAsync(cancellationToken);
        return mapper.Map<List<OfficeModel>>(officeEntities);
    }

    public async Task<OfficeModel> CreateAsync(CreateOfficeRequest request, CancellationToken cancellationToken)
    {
        var officeEntity = mapper.Map<OfficeEntity>(request);
        officeEntity.CreatedAt = DateTime.UtcNow;
        var createdOfficeEntity = await officeRepository.CreateAsync(officeEntity, cancellationToken);
        return mapper.Map<OfficeModel>(createdOfficeEntity);
    }

    public async Task<OfficeModel> UpdateAsync(UpdateOfficeRequest request, CancellationToken cancellationToken)
    {
        var existingOfficeEntity = await officeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingOfficeEntity == null)
        {
            logger.LogWarning(NotificationMessages.NotFoundErrorMessage, request.Id);
            throw new NotFoundException(string.Format(NotificationMessages.NotFoundErrorMessage, request.Id));
        }

        var officeEntityToUpdate = mapper.Map(request, existingOfficeEntity);
        officeEntityToUpdate.UpdatedAt = DateTime.UtcNow;
        var updatedOfficeEntity = await officeRepository.UpdateAsync(officeEntityToUpdate, cancellationToken);
        return mapper.Map<OfficeModel>(updatedOfficeEntity);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var existingOfficeEntity = await officeRepository.GetByIdAsync(id, cancellationToken);
        if (existingOfficeEntity == null)
        {
            logger.LogWarning(string.Format(NotificationMessages.NotFoundErrorMessage, id));
            return false;
        }

        var isDeleted = await officeRepository.DeleteAsync(id, cancellationToken);
        return isDeleted;
    }

    public async Task<List<string>> GetAllCitiesAsync(CancellationToken cancellationToken)
    {
        var officeEntities = await officeRepository.GetAllCitiesAsync(cancellationToken);
        var t = await officeRepository.GetAllCitiesAndStreetsAsync(cancellationToken);
        return officeEntities;
    }
}