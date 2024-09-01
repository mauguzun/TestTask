using System.Globalization;
using System.Net;
using OneOf;
using TestTask.Application.Inerfaces;
using TestTask.Domain.Entites;
using TestTask.Infrastructure.Inerfaces.Repositories;
using TestTask.Infrastructure.Inerfaces.Services;

namespace TestTask.Application.Implementations;

public class WeatherService : IWeatherService
{
    private readonly IBlobRepository _blobRepository;
    private readonly IOpenWeatherService _openWeatherService;
    private readonly IStorageRepository _storageRepository;

    public WeatherService(IOpenWeatherService openWeatherService, IBlobRepository blobRepository,
        IStorageRepository storageRepository)
    {
        (_openWeatherService, _blobRepository, _storageRepository) =
            (openWeatherService, blobRepository, storageRepository);
    }

    public async Task FetchAndStoreWeatherDataAsync(string location, CancellationToken cancellationToken)
    {
        var httpResponse = await _openWeatherService.GetWeatherAsync(location, cancellationToken);

        var blobID = Guid.NewGuid().ToString();
        var entity = new WeatherLog(blobID, (int)httpResponse.StatusCode);

        await _blobRepository.SaveBlobDataAsync(blobID, httpResponse.Content, cancellationToken);
        var azureResponse = await _storageRepository.SaveWeatherDataAsync(entity, cancellationToken);

        if (azureResponse.IsError)
        {
            var message = azureResponse.ReasonPhrase ?? "No additional information provided.";
            throw (HttpStatusCode)azureResponse.Status switch
            {
                HttpStatusCode.Unauthorized => new UnauthorizedAccessException(
                    $"Unauthorized access to Azure storage: {message}"),
                _ => new Exception($"Unexpected error with status code {azureResponse.Status}: {message}")
                //all others 
            };
        }
    }

    public async Task<string?> GetPayload(string id, CancellationToken cancellationToken)
    {
        var downloadResult = await _blobRepository.GetBlobById(id, cancellationToken);

        if (downloadResult is null)
            return null;


        HttpContent httpContent = new ByteArrayContent(downloadResult.Value.Content.ToArray());
        return await httpContent.ReadAsStringAsync();
    }

    public OneOf<IList<WeatherLog>, string> GetWeatherLogs(string from, string to, CancellationToken cancellationToken)
    {
        const string dateFormat = "yyyy-MM-ddTHH:mm";

        if (!DateTimeOffset.TryParseExact(from, dateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal, out var dateFrom) ||
            !DateTimeOffset.TryParseExact(to, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal,
                out var dateTo))
            return "Invalid date format. Use 'yyyy-MM-ddTHH:mm'.";

        if (dateTo < dateFrom)
            return "The 'to' date must be later than the 'from' date.";
        if ((dateTo - dateFrom).TotalHours > 1)
            return "The date range should not exceed 1 hour.";


        dateTo = dateTo.AddSeconds(59).AddMilliseconds(999);

        var logs = _storageRepository.GetLogByDataRange(dateFrom, dateTo, cancellationToken);
        return OneOf<IList<WeatherLog>, string>.FromT0(logs);
    }
}