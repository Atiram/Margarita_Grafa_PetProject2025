using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;
using ClinicService.DAL.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.DAL.Repositories;

public class DoctorRepository(ClinicDbContext context) : GenericRepository<DoctorEntity>(context), IDoctorRepository
{
    public async Task<List<DoctorEntity>> GetAllAsync(
        // string sortParameter,  
        bool isDescending,
        int pageNumber,
        int pageSize,
        string s,
        CancellationToken cancellationToken)
    {
        var totalCount = context.Set<DoctorEntity>().Count();

        var entities = context.Set<DoctorEntity>();

        var sortedEntities = isDescending
            ? entities.OrderByDescending(e => e.LastName)
            : entities.OrderBy(e => e.LastName);
        var w = sortedEntities.Where()
        //var r = context.Set<DoctorEntity>()
        //    .Skip((pageNumber - 1) * pageSize)
        //    .Take(pageSize);
        var r = sortedEntities
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        //PagedResult<DoctorEntity> p = new PagedResult<DoctorEntity>()
        //{
        //    PageSize = pageSize,
        //    TotalCount = totalCount,
        //    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
        //    Result = r.ToList()
        //};


        return await r.ToListAsync();
    }
}