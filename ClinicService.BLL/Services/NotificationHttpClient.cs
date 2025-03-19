using System.Text;
using Clinic.DOMAIN;
using ClinicService.BLL.Services.Interfaces;
using Newtonsoft.Json;

namespace ClinicService.BLL.Services;
public class NotificationHttpClient : INotificationHttpClient
{
    private const string JsonContentType = "application/json";
    private string eventUrl;
    public NotificationHttpClient(string eventUrl)
    {
        this.eventUrl = eventUrl;
    }
    public async Task SendEventRequest(CreateEventMail createEventMail, CancellationToken cancellationToken)
    {
        HttpClient httpClient = new HttpClient();
        var content = new StringContent(JsonConvert.SerializeObject(createEventMail), Encoding.UTF8, JsonContentType);
        HttpResponseMessage response = await httpClient.PostAsync(eventUrl, content, cancellationToken);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
    }
}
