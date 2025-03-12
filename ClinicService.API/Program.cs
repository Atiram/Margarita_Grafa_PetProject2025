using System.Reflection;
using System.Text.Json.Serialization;
using ClinicService.API.Middleware;
using ClinicService.API.Utilities.Mapping;
using ClinicService.BLL.DI;

namespace ClinicServiceApi
{
    public partial class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var services = builder.Services;
            var configuration = builder.Configuration;

            services.AddControllers()
                .AddJsonOptions(options =>
                    {
                        var enumConverter = new JsonStringEnumConverter();
                        var jsonOptions = options.JsonSerializerOptions;
                        jsonOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                        jsonOptions.Converters.Add(enumConverter);
                    });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthorization();

            app.MapControllers();

            app.UseCors("AllowReactApp");

            app.Run();
        }
    }
}