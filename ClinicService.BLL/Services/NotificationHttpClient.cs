using System.Text;
using Clinic.DOMAIN;
using ClinicService.BLL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ClinicService.BLL.Services;
public class NotificationHttpClient : INotificationHttpClient
{
    private const string JsonContentType = "application/json";
    private string eventUrl;
    private HttpClient httpClient;

    public NotificationHttpClient(IConfiguration configuration)
    {
        this.eventUrl = configuration.GetSection("EventUrl").Value ?? throw new ArgumentException("Section 'EventUrl' is missing or empty in configuration.");
        httpClient = new HttpClient();
    }
    public async Task SendEventRequest(CreateEventMail createEventMail, CancellationToken cancellationToken)
    {
        var content = new StringContent(JsonConvert.SerializeObject(createEventMail), Encoding.UTF8, JsonContentType);
        HttpResponseMessage response = await httpClient.PostAsync(eventUrl, content, cancellationToken);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
    }
}
