using OfficeService.DAL.Entities;

namespace OfficeService.DAL.Repositories.Interfaces;
public interface IOfficeRepository
{
    Task<OfficeEntity> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task<List<OfficeEntity>> GetAllAsync(CancellationToken cancellationToken);

    Task<OfficeEntity> CreateAsync(OfficeEntity officeEntity, CancellationToken cancellationToken);

    Task<OfficeEntity?> UpdateAsync(OfficeEntity officeEntity, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
}