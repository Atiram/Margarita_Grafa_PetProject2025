using System.Net.Http.Headers;
using AutoMapper;
using Clinic.Domain;
using ClinicService.BLL.Models;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace ClinicService.BLL.Services;
public class GeneratePdfService(IAppointmentResultRepository appointmentResultRepository, IMapper mapper, IConfiguration configuration, IHttpClientFactory httpClientFactory) : IGeneratePdfService
{
    private const string FileServiceSectionName = "FileServiceBaseUrl";
    private readonly string fileServiceBaseUrl = configuration.GetSection(FileServiceSectionName).Value ??
        throw new ArgumentException(string.Format(NotificationMessages.SectionMissingErrorMessage, FileServiceSectionName));

    private HttpClient httpClient = httpClientFactory.CreateClient("FileService");
    private const string fileContentType = "application/pdf";
    private const string fileDownloadName = "AppointmentResult_{0}.pdf";

    public async Task<byte[]> SaveToPdfAsync(Guid id, CancellationToken cancellationToken)
    {
        var appointmentResultEntity = await appointmentResultRepository.GetByIdAsync(id, cancellationToken);
        if (appointmentResultEntity == null)
        {
            throw new Exception(string.Format(NotificationMessages.NotFoundErrorMessage, id));
        }
        var appointmentResultModel = mapper.Map<AppointmentResultModel>(appointmentResultEntity);
        return GeneratePdfBytes(appointmentResultModel);
    }

    public async Task UploadPdfToStorageAsync(byte[] pdfBytes, Guid referenseItemId, CancellationToken cancellationToken)
    {

        using var content = new MultipartFormDataContent();

        // Добавляем byte[] как ByteArrayContent
        //var byteArrayContent = new ByteArrayContent(pdfBytes);
        //byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(fileContentType);
        //content.Add(byteArrayContent, "inMemoryFile", blobName); // "fileBytes" - имя параметра, ожидаемое FileService


        // Создаем StreamContent из byte[]
        using var streamContent = new StreamContent(new MemoryStream(pdfBytes));

        // **Ключевой момент:** Устанавливаем Content-Type
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

        // Добавляем StreamContent в MultipartFormDataContent
        content.Add(streamContent, "inMemoryFile", $"{referenseItemId}.pdf"); // "file" - имя параметра, ожидаемое сервером



        // content.Add(new ByteArrayContent(pdfBytes), "inMemoryFile");
        //content.Add(new StringContent(pdfBytes.ToString()), "inMemoryFile");
        content.Add(new StringContent(referenseItemId.ToString()), "referenceItemId");
        content.Add(new StringContent("Pdf"), "documentType");
        content.Add(new StringContent($"{referenseItemId}.pdf"), "blobName");

        HttpResponseMessage response = await httpClient.PostAsync(fileServiceBaseUrl, content, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    //public async Task<string> CreateAndSavePdfAsync(Guid id, CancellationToken cancellationToken)
    //{
    //    var pdfBytes = await SaveToPdfAsync(id, cancellationToken);
    //    var blobName = string.Format(fileDownloadName, id); // Use the consistent file name
    //    await UploadPdfToStorageAsync(pdfBytes, blobName, cancellationToken);
    //    return blobName; // Return the blobName (or the StorageLocation if your FileService returns it)
    //}

    //private async Task UploadFileAsync(byte[] fileBytes, Guid referenceItemId, CancellationToken cancellationToken)
    //{
    //    if (fileBytes == null || fileBytes.Length == 0)
    //    {
    //        throw new ValidationException(NotificationMessages.NotFoundErrorMessage);
    //    }
    //    using var content = new MultipartFormDataContent();


    //    var uploadResponse = await httpClient.PostAsync(
    //        fileServiceBaseUrl,
    //        content,
    //        cancellationToken);

    //    uploadResponse.EnsureSuccessStatusCode();

    //}

    private byte[] GeneratePdfBytes(AppointmentResultModel result)
    {
        using (var document = new PdfDocument())
        {
            var page = document.AddPage();
            using (var gfx = XGraphics.FromPdfPage(page))
            {
                var font = new XFont("Arial", 12, XFontStyleEx.Regular);
                double y = 50;
                double x = 50;
                gfx.DrawString($"ID: {result.Id}", font, XBrushes.Black, new XRect(50, y, x, 20), XStringFormats.TopLeft);
                y += 20;
                gfx.DrawString($"Created At: {result.CreatedAt}", font, XBrushes.Black, new XRect(50, y, x, 20), XStringFormats.TopLeft);
                y += 20;
                gfx.DrawString($"Complaints: {result.Complaints}", font, XBrushes.Black, new XRect(50, y, x, 20), XStringFormats.TopLeft);
                y += 20;
                gfx.DrawString($"Recommendations: {result.Recommendations}", font, XBrushes.Black, new XRect(50, y, x, 20), XStringFormats.TopLeft);
                y += 20;
                gfx.DrawString($"Conclusion: {result.Conclusion}", font, XBrushes.Black, new XRect(50, y, x, 20), XStringFormats.TopLeft);
                y += 20;
            }

            using (var stream = new MemoryStream())
            {
                document.Save(stream);
                return stream.ToArray();
            }
        }
    }
}
