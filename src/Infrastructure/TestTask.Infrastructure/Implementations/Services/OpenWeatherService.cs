using Microsoft.Extensions.Configuration;
using TestTask.Infrastructure.Inerfaces.Services;

namespace TestTask.Infrastructure.Implementations.Services
{
    public class OpenWeatherService : IOpenWeatherService
    {
        private readonly string _apiKey;
        private readonly int _timeout;
        public OpenWeatherService(IConfiguration configuration)=>
            (_apiKey, _timeout) = (configuration["ApiKey"], configuration.GetValue<int>("TimeOut"));

        public async Task<HttpResponseMessage> GetWeatherAsync(string location, CancellationToken cancellationToken)
        {
            using HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(_timeout) };
            var response =  await  client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={location}&appid={_apiKey}", cancellationToken);

            return response;
        }
        
    }
}
