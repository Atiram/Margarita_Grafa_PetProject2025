using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

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
                var dbContextService = services.SingleOrDefault(x =>
                    x.ServiceType == typeof(ClinicDbContext));
                services.Remove(dbContextService!);

                services.AddDbContext<ClinicDbContext>(options => options.UseInMemoryDatabase("TestDb"));

                //services.AddMassTransitTestHarness(x =>
                //{
                //    x.UsingInMemory();
                //});

                //services.AddScoped(serviceProvider => A.Fake<DoctorService>());
                services.AddScoped(serviceProvider =>
                {
                    var mockDoctorRepository = new Mock<IDoctorRepository>();
                    return mockDoctorRepository.Object;
                });
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
