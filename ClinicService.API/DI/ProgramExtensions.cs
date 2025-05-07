using ClinicService.API.Validators;
using ClinicService.BLL.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Polly;
using Polly.Extensions.Http;

namespace ClinicService.API.DI;

public static class ProgramExtensions
{
    public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<CreateDoctorRequestValidator>()
            .AddValidatorsFromAssemblyContaining<CreatePatientRequestValidator>()
            .AddHttpClient("MyHttpClient")
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy());

        services.AddHangfire(hangfireConfiguration => hangfireConfiguration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

        services.AddHangfireServer();
    }

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}