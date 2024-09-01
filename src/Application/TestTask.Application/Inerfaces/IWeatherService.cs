using OneOf;
using TestTask.Domain.Entites;

namespace TestTask.Application.Inerfaces;

public interface IWeatherService
{
    Task FetchAndStoreWeatherDataAsync(string location, CancellationToken cancellationToken);

    Task<string?> GetPayload(string id, CancellationToken cancellationToken);
    OneOf<IList<WeatherLog>, string> GetWeatherLogs(string from, string to, CancellationToken cancellationToken);
}