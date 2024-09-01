using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using TestTask.Api.AzureFunctions;
using TestTask.Application.Implementations;
using TestTask.Application.Inerfaces;
using TestTask.Infrastructure.Implementations.Repositories;
using TestTask.Infrastructure.Implementations.Services;
using TestTask.Infrastructure.Inerfaces.Repositories;
using TestTask.Infrastructure.Inerfaces.Services;

[assembly: FunctionsStartup(typeof(Startup))]

namespace TestTask.Api.AzureFunctions;
public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        builder.Services.AddSingleton<IConfiguration>(configuration);

        //repositories
        builder.Services.AddScoped<IBlobRepository, BlobRepository>();
        builder.Services.AddScoped<IStorageRepository, StorageRepository>();
        // infra
        builder.Services.AddScoped<IOpenWeatherService, OpenWeatherService>();
        // application
        builder.Services.AddScoped<IWeatherService, WeatherService>();

    }
}
