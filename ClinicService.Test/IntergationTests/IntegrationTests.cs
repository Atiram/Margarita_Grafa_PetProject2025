using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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


    public async Task<Guid> AddToContext<T>(T entity) where T : DoctorEntity
    {
        var dbSet = Context.Set<T>();
        await dbSet.AddAsync(entity);
        await Context.SaveChangesAsync();

        return entity.Id;
    }
}
