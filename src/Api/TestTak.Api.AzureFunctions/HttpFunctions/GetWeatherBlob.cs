using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Threading;
using System.Threading.Tasks;
using TestTask.Application.Inerfaces;

namespace JustForTest
{
    public class GetWeatherBlob
    {
        private readonly IWeatherService _weatherService;
        public GetWeatherBlob(IWeatherService weatherService) => _weatherService = weatherService;

        [FunctionName("GetWeatherBlob")]
        public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetWeatherBlob/{id}")] HttpRequest req,
        string id,
        CancellationToken cancellationToken)=>
            (await _weatherService.GetPayload(id, cancellationToken)) is string payload
            ? new OkObjectResult(payload)
            : new NotFoundObjectResult($"Result with {id} not found");
      
    }
}
