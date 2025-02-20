using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.DAL.Repositories;

public class GenericRepository<TEntity>(ClinicDbContext context) : IGenericRepository<TEntity> where TEntity : GenericEntity
{
    public ValueTask<TEntity?> GetByIdAsync(Guid id)
    {
        return context.Set<TEntity>().FindAsync([id]);
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        context.Update(entity);
        await context.SaveChangesAsync();

        await context.Entry(entity).ReloadAsync();

        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await context.Set<TEntity>().FindAsync([id]);

        if (entity is not null)
        {
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();

            return true;
        }

        return false;
    }
}
