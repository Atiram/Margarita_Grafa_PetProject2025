namespace ClinicService.DAL.Repositories.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    ValueTask<TEntity?> GetByIdAsync(Guid id);

    Task<TEntity> CreateAsync(TEntity entity);

    Task<TEntity> UpdateAsync(TEntity entity);

    Task<bool> DeleteAsync(Guid id);
}