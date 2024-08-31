using Azure;
using Azure.Data.Tables;
using TestTask.Core.Entites;

namespace TestTask.Infrastructure.Inerfaces.Repositories
{
    public interface IStorageRepository
    {
        Task<Response> SaveWeatherDataAsync(WeatherLog data, CancellationToken cancellationToken);
        IList<WeatherLog> GetLogByDataRange(DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken);
    }
}
