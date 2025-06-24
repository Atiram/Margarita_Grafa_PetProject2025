using OfficeService.BLL.DI;
using OfficeService.DAL.MongoDb;

namespace OfficeService.API.DI;

public static class ProgramExtensions
{
    public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoSettings"))
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
            .RegisterBusinessLogicServices(configuration);
    }
}
