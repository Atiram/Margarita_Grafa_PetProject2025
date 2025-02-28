using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.DAL.Repositories;

public class GenericRepository<TEntity>(ClinicDbContext context) : IGenericRepository<TEntity> where TEntity : GenericEntity
{
    public ValueTask<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return context.Set<TEntity>().FindAsync([id], cancellationToken);
    }

    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await context.Set<TEntity>().AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        context.Update(entity);
        await context.SaveChangesAsync(cancellationToken);

        await context.Entry(entity).ReloadAsync(cancellationToken);

        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await context.Set<TEntity>().FindAsync([id], cancellationToken);

        if (entity is not null)
        {
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }

        return false;
    }
}
