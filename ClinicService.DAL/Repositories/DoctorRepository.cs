using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Enums;
using ClinicService.DAL.Repositories.Interfaces;
using ClinicService.DAL.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.DAL.Repositories;

public class DoctorRepository(ClinicDbContext context) : GenericRepository<DoctorEntity>(context), IDoctorRepository
{
    private static IQueryable<DoctorEntity> AddOrdering(IQueryable<DoctorEntity> query, DoctorSortingParams? sortBy, SortOrderType? sortOrder)
    {
        switch (sortBy)
        {
            case DoctorSortingParams.FirstName:
                {
                    if (sortOrder is null || sortOrder == SortOrderType.Asc) return query.OrderBy(x => x.FirstName);
                    return query.OrderByDescending(x => x.FirstName);
                }
            case DoctorSortingParams.LastName:
                {
                    if (sortOrder is null || sortOrder == SortOrderType.Asc) return query.OrderBy(x => x.LastName);
                    return query.OrderByDescending(x => x.LastName);
                }
            case DoctorSortingParams.MiddleName:
                {
                    if (sortOrder is null || sortOrder == SortOrderType.Asc) return query.OrderBy(x => x.MiddleName);
                    return query.OrderByDescending(x => x.MiddleName);
                }
            case DoctorSortingParams.DateOfBirth:
                {
                    if (sortOrder is null || sortOrder == SortOrderType.Asc) return query.OrderBy(x => x.DateOfBirth);
                    return query.OrderByDescending(x => x.DateOfBirth);
                }
            case DoctorSortingParams.Email:
                {
                    if (sortOrder is null || sortOrder == SortOrderType.Asc) return query.OrderBy(x => x.Email);
                    return query.OrderByDescending(x => x.Email);
                }
            case DoctorSortingParams.Specialization:
                {
                    if (sortOrder is null || sortOrder == SortOrderType.Asc) return query.OrderBy(x => x.Specialization);
                    return query.OrderByDescending(x => x.Specialization);
                }
            case DoctorSortingParams.Office:
                {
                    if (sortOrder is null || sortOrder == SortOrderType.Asc) return query.OrderBy(x => x.Office);
                    return query.OrderByDescending(x => x.Office);
                }
            case DoctorSortingParams.CareerStartYear:
                {
                    if (sortOrder is null || sortOrder == SortOrderType.Asc) return query.OrderBy(x => x.CareerStartYear);
                    return query.OrderByDescending(x => x.CareerStartYear);
                }
            case DoctorSortingParams.Status:
                {
                    if (sortOrder is null || sortOrder == SortOrderType.Asc) return query.OrderBy(x => x.Status);
                    return query.OrderByDescending(x => x.Status);
                }
            case DoctorSortingParams.CreatedAt:
                {
                    if (sortOrder is null || sortOrder == SortOrderType.Asc) return query.OrderBy(x => x.CreatedAt);
                    return query.OrderByDescending(x => x.CreatedAt);
                }
            case null: return query.OrderByDescending(x => x.CreatedAt);
            default: return query;
        }
    }

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

        var totalCount = await filteredEntities.CountAsync();

        PagedResult<DoctorEntity> pagedResult = new PagedResult<DoctorEntity>()
        {
            PageSize = getAllDoctorsParams.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / getAllDoctorsParams.PageSize),
        };

        pagedResult.Results = await filteredEntities
          .Skip((getAllDoctorsParams.PageNumber - 1) * getAllDoctorsParams.PageSize)
          .Take(getAllDoctorsParams.PageSize)
          .ToListAsync();

        return pagedResult;
    }
}