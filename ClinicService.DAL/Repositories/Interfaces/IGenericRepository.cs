using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClinicService.DAL.Repositories.Interfaces
{
    internal interface IGenericRepository<TEntity> where TEntity : class 
    {
        Task<TEntity?> GetByIdAsync(int id);

        Task<TEntity> CreateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(int id);
    }
}
