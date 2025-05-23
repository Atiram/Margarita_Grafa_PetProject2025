using AuthenticationService.API.DI;
using AuthenticationService.API.Middleware;
using AuthenticationService.DAL.MongoDb;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
                loggerConfig.WriteTo.Console());

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthorization();

builder.Services.AddAuthorization();

builder.Services.RegisterDependencies(builder.Configuration);

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoSettings"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthenticationService.API V1"));
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
