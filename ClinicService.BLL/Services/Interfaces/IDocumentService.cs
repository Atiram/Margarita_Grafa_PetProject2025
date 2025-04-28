using Microsoft.AspNetCore.Http;

namespace ClinicService.BLL.Services.Interfaces;
public interface IDocumentService
{
    Task<string> GetPhotoAsync(Guid doctorId, CancellationToken cancellationToken);
    Task UploadPhotoAsync(Guid doctorId, IFormFile? photoFile, CancellationToken cancellationToken);
    Task DeletePhotoAsync(Guid doctorId, CancellationToken cancellationToken);
}
