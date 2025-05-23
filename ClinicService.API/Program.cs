using System.Reflection;
using System.Text.Json.Serialization;
using ClinicService.API.DI;
using ClinicService.API.Middleware;
using ClinicService.API.Utilities.Mapping;
using ClinicService.BLL.DI;
using Hangfire;
using Serilog;

namespace ClinicServiceApi
{
    public partial class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;
            var configuration = builder.Configuration;

            builder.Host.UseSerilog((context, loggerConfig) =>
                loggerConfig.WriteTo.Console());

            services.AddControllers()
                .AddJsonOptions(options =>
                    {
                        var enumConverter = new JsonStringEnumConverter();
                        var jsonOptions = options.JsonSerializerOptions;
                        jsonOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                        jsonOptions.Converters.Add(enumConverter);
                    });

            services.RegisterDependencies(configuration);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.RegisterBusinessLogicServices(configuration);
            services.AddAutoMapper(Assembly.GetAssembly(typeof(AppMappingProfile)));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHangfireDashboard();

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthorization();

            app.MapControllers();

            app.UseCors("AllowReactApp");

            app.Run();
        }
    }
}