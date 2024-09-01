using Azure;
using TestTask.Domain.Entites;

namespace TestTask.Infrastructure.Inerfaces.Repositories;

public interface IStorageRepository
{
    Task<Response> SaveWeatherDataAsync(WeatherLog data, CancellationToken cancellationToken);
    IList<WeatherLog> GetLogByDataRange(DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken);
}