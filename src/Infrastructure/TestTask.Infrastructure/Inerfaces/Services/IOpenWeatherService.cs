namespace TestTask.Infrastructure.Inerfaces.Services
{
    public interface IOpenWeatherService
    {
        public Task<HttpResponseMessage> GetWeatherAsync(string location, CancellationToken cancellationToken);
    }
}
