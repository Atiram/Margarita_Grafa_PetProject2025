using System.Reflection;
using ClinicService.BLL.DI;
using ClinicService.DAL.DI;
using ClinicService.BLL.Utilities.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var servises = builder.Services;
var configuration = builder.Configuration;

servises.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
servises.AddEndpointsApiExplorer();
servises.AddSwaggerGen();

servises.AddAutoMapper(Assembly.GetAssembly(typeof(AppMappingProfile)));
servises.RegisterDataRepositories(configuration);
servises.RegisterBusinessLogicServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
