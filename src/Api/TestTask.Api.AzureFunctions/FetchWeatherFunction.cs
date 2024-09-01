using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using TestTask.Application.Inerfaces;

namespace TestTask.Api.AzureFunctions;

public class WeatherFunction
{
    private readonly IWeatherService _weatherService;

    public WeatherFunction(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [FunctionName("FetchWeatherFunctionTimer")]
    public async Task Run(
        [TimerTrigger("0 * * * * *")] TimerInfo timerInfo,
        CancellationToken cancellationToken)
    {
        var location = "london,uk"; //can get somewhere 
        await _weatherService.FetchAndStoreWeatherDataAsync(location, cancellationToken);
    }
}