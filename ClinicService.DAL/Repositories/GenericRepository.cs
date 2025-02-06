using ClinicService.DAL.Data;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.DAL.Repositories
{
    internal class GenericRepository<TEntity>(ClinicDbContext context) : IGenericRepository<TEntity> where TEntity : class, new()
    {
        //private readonly ClinicDbContext _context;

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await context.Set<TEntity>().FindAsync([id]);
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

        public async Task<bool> DeleteAsync(int id)
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
}
