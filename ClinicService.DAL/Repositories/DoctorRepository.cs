using System.Linq.Expressions;
using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Enums;
using ClinicService.DAL.Repositories.Interfaces;
using ClinicService.DAL.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.DAL.Repositories;

public class DoctorRepository(ClinicDbContext context) : GenericRepository<DoctorEntity>(context), IDoctorRepository
{
    public async Task<PagedResult<DoctorEntity>> GetAllAsync(GetAllDoctorsParams getAllDoctorsParams, CancellationToken cancellationToken)
    {
        var entities = context.Set<DoctorEntity>();

        SortOrderType sortOrder = getAllDoctorsParams.IsDescending ? SortOrderType.Desc : SortOrderType.Asc;
        var sortedEntities = AddOrdering(entities, getAllDoctorsParams.SortParameter, sortOrder);

        var filteredEntities = getAllDoctorsParams.SearchValue != null
          ? sortedEntities.Where(doctor =>
                  doctor.FirstName.Contains(getAllDoctorsParams.SearchValue) ||
                  doctor.LastName.Contains(getAllDoctorsParams.SearchValue))
          : sortedEntities;

        var totalCount = await filteredEntities.CountAsync(cancellationToken);

        PagedResult<DoctorEntity> pagedResult = new PagedResult<DoctorEntity>()
        {
            PageSize = getAllDoctorsParams.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / getAllDoctorsParams.PageSize),
        };

        pagedResult.Results = await filteredEntities
          .Skip((getAllDoctorsParams.PageNumber - 1) * getAllDoctorsParams.PageSize)
          .Take(getAllDoctorsParams.PageSize)
          .AsNoTracking()
          .ToListAsync(cancellationToken);

        return pagedResult;
    }
    private static IQueryable<DoctorEntity> AddOrdering(IQueryable<DoctorEntity> query, DoctorSortingParams? sortBy, SortOrderType? sortOrder)
    {
        return sortBy switch
        {
            DoctorSortingParams.FirstName => ApplyOrdering(query, x => x.FirstName, sortOrder),
            DoctorSortingParams.LastName => ApplyOrdering(query, x => x.LastName, sortOrder),
            DoctorSortingParams.MiddleName => ApplyOrdering(query, x => x.MiddleName ?? string.Empty, sortOrder),
            DoctorSortingParams.DateOfBirth => ApplyOrdering(query, x => x.DateOfBirth, sortOrder),
            DoctorSortingParams.Email => ApplyOrdering(query, x => x.Email, sortOrder),
            DoctorSortingParams.Specialization => ApplyOrdering(query, x => x.Specialization, sortOrder),
            DoctorSortingParams.Office => ApplyOrdering(query, x => x.Office, sortOrder),
            DoctorSortingParams.CareerStartYear => ApplyOrdering(query, x => x.CareerStartYear, sortOrder),
            DoctorSortingParams.Status => ApplyOrdering(query, x => x.Status, sortOrder),
            DoctorSortingParams.CreatedAt => ApplyOrdering(query, x => x.CreatedAt, sortOrder),
            null => query.OrderByDescending(x => x.CreatedAt),
            _ => query
        };
    }
    private static IQueryable<DoctorEntity> ApplyOrdering(
        IQueryable<DoctorEntity> query,
        Expression<Func<DoctorEntity, object>> keySelector,
        SortOrderType? sortOrder)
    {
        return sortOrder is null || sortOrder == SortOrderType.Asc
            ? query.OrderBy(keySelector)
            : query.OrderByDescending(keySelector);
    }
}