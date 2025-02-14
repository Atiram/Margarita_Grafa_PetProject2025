using System.Text;
using ClinicService.API.ViewModels;
using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
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

    public HttpRequestMessage? AddContent(DoctorViewModel viewModel, HttpRequestMessage? requestMessage)
    {
        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");

        return requestMessage;
    }

    public async Task<DoctorViewModel> SendRequest(DoctorViewModel doctorViewModel)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7105/Doctor");
        request.Content = new StringContent(JsonConvert.SerializeObject(doctorViewModel), Encoding.UTF8, "application/json");
        var actualResult = await Client.SendAsync(request);
        var responseResult = JsonConvert.DeserializeObject<DoctorViewModel>(actualResult.Content.ReadAsStringAsync().Result);
        return responseResult;
    }
    public async Task<Guid> AddToContext<T>(T entity) where T : DoctorEntity
    {
        var dbSet = Context.Set<T>();
        await dbSet.AddAsync(entity);
        await Context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<bool> IsExist<T>(T entity) where T : DoctorEntity
    {
        var dbSet = Context.Set<T>();
        var b = await dbSet.ContainsAsync(entity);
        await Context.SaveChangesAsync();

        return b;
    }
}
