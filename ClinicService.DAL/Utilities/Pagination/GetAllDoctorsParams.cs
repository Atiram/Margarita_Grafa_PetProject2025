using ClinicService.DAL.Enums;

namespace ClinicService.DAL.Utilities.Pagination;
public class GetAllDoctorsParams
{
    public bool IsDescending { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public DoctorSortingParams SortParameter { get; set; }
    public string? SearchField { get; set; }
    public string? SearchValue { get; set; }
}