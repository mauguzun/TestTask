using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using OneOf.Types;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestTask.Application.Inerfaces;
namespace JustForTest
{
    public class GetWeatherLogs
    {
        private readonly IWeatherService _weatherService;
        public GetWeatherLogs(IWeatherService weatherService) => _weatherService = weatherService;

        [FunctionName("GetWeatherLogs")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetWeatherLogs/{from}/{to}")] HttpRequest req,
            string from, string to, CancellationToken token)
        {
            var weatherLogs = _weatherService.GetWeatherLogs(from, to, token);

            return weatherLogs.Match<IActionResult>(
                   weatherData => new OkObjectResult(weatherData),
                   error => new BadRequestObjectResult(error)
            );
        }
    }
}
