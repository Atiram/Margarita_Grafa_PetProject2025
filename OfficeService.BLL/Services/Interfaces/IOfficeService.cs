using OfficeService.BLL.Models;
using OfficeService.BLL.Models.Requests;

namespace OfficeService.BLL.Services.Interfaces;
public interface IOfficeService
{
    Task<OfficeModel> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task<List<OfficeModel>> GetAllAsync(CancellationToken cancellationToken);

    Task<OfficeModel> CreateAsync(CreateOfficeRequest request, CancellationToken cancellationToken);

    Task<OfficeModel> UpdateAsync(UpdateOfficeRequest request, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);

    Task<List<string>> GetAllCitiesAsync(CancellationToken cancellationToken);
}
