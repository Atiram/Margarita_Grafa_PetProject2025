using System.Text;
using ClinicService.API.ViewModels;
using ClinicService.DAL.Data;
using ClinicServiceApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;

namespace ClinicService.Test.IntergationTests;
public class IntegrationTests
{
    protected TestServer Server { get; }
    protected HttpClient Client { get; }
    protected ClinicDbContext Context { get; }
    protected WebApplicationFactory<Program> Factory { get; }
    private const string JsonContentType = "application/json";
    private const string UrlPost = "https://localhost:7105/Doctor";

    public IntegrationTests()
    {
        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<ClinicDbContext>>();

                services.AddDbContext<ClinicDbContext>(options => options.UseInMemoryDatabase("TestDb"));
            }));
        Server = Factory.Server;
        Client = Server.CreateClient();
        Context = Factory.Services.CreateScope().ServiceProvider.GetService<ClinicDbContext>()!;
    }

    public static HttpRequestMessage AddContent(DoctorViewModel viewModel, HttpRequestMessage requestMessage)
    {
        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, JsonContentType);
        return requestMessage;
    }
    public async Task<HttpResponseMessage> SendPostRequest(DoctorViewModel viewModel)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, UrlPost);
        var actualRequest = AddContent(viewModel, request);
        return await Client.SendAsync(request);
    }
    public static DoctorViewModel? GetResponseResult(HttpResponseMessage responseMessage)
    {
        return JsonConvert.DeserializeObject<DoctorViewModel>(responseMessage.Content.ReadAsStringAsync().Result) ?? null;
    }
}
