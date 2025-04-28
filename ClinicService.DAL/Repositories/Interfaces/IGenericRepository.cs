namespace ClinicService.DAL.Repositories.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    ValueTask<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken);

    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}