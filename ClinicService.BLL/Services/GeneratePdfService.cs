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
public class GeneratePdfService(IAppointmentResultRepository appointmentResultRepository,
    IMapper mapper,
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory) : IGeneratePdfService

{
    private const string FileServiceSectionName = "FileServiceBaseUrl";
    private const string fileContentType = "application/pdf";
    private readonly string fileServiceBaseUrl = configuration.GetSection(FileServiceSectionName).Value ??
        throw new ArgumentException(string.Format(NotificationMessages.SectionMissingErrorMessage, FileServiceSectionName));
    private HttpClient httpClient = httpClientFactory.CreateClient("FileService");

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
        var fileContent = new ByteArrayContent(pdfBytes);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(fileContentType);
        content.Add(fileContent, "file", $"{referenseItemId}.pdf");
        content.Add(new StringContent(referenseItemId.ToString()), "referenceItemId");
        content.Add(new StringContent("PdfFile"), "documentType");
        content.Add(new StringContent($"{referenseItemId}.pdf"), "blobName");

        HttpResponseMessage response = await httpClient.PostAsync(fileServiceBaseUrl, content, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

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
