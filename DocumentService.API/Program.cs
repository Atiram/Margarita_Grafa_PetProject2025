using DocumentService.BBL.DI;
using DocumentService.DAL.MongoDb;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterBusinessLogicServices(builder.Configuration);
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoSettings"));

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.WriteTo.Console());

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
