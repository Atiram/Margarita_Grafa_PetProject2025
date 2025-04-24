namespace ClinicService.BLL.Services.Interfaces;
public interface IGeneratePdfService
{
    Task<byte[]> SaveToPdfAsync(Guid id, CancellationToken cancellationToken);
    Task UploadPdfToStorageAsync(byte[] pdfBytes, Guid referenseItemId, CancellationToken cancellationToken);

}
