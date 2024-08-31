using Microsoft.Azure.WebJobs;
using System.Threading;
using System.Threading.Tasks;
using TestTask.Application.Inerfaces;
using TestTask.Infrastructure.Inerfaces.Services;

namespace TestTask.Api.AzureFunctions
{
    public class WeatherFunction
    {
        private readonly IWeatherService _weatherService;
        public WeatherFunction(IWeatherService weatherService) => _weatherService = weatherService;

        [FunctionName("FetchWeatherFunctionTimer")]
        public async Task Run(
            [TimerTrigger("0 * * * * *")] TimerInfo timerInfo,
            CancellationToken cancellationToken)
        {
            string location = "london,uk"; //can get somewhere 
             await _weatherService.FetchAndStoreWeatherDataAsync(location, cancellationToken);

        }
    }
}
