using Microsoft.AspNetCore.Mvc;
using Infra.Clients.WeatherForecastApi;
using Infra.Clients.ExternalServiceApi;
using Domain.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExternalServiceController : ControllerBase
    {
        private readonly IWeatherForecastApiClient _weatherForecastApiClient;
        private readonly IExternalServiceApiClient _externalServiceClient;

        public ExternalServiceController(
            IWeatherForecastApiClient weatherForecastApiClient,
            IExternalServiceApiClient externalServiceClient)
        {
            _weatherForecastApiClient = weatherForecastApiClient
                ?? throw new ArgumentNullException(nameof(weatherForecastApiClient));

            _externalServiceClient = externalServiceClient
                ?? throw new ArgumentNullException(nameof(externalServiceClient));
        }

        [HttpGet("/weather-forecast")]
        public async Task<IActionResult> GetWeatherForecast()
        {
            var weatherForecast = await _weatherForecastApiClient.GetWeatherForecastsAsync();
            return Ok(weatherForecast);
        }

        [HttpGet("/weather-forecast/broken")]
        public async Task<IActionResult> GetWeatherForecastBroken()
        {
            await _weatherForecastApiClient.GetBrokenAsync();
            return Ok();
        }

        [HttpPost("/create-something")]
        public async Task<IActionResult> CreateSomething([FromBody] Something something)
        {
            await _externalServiceClient.CreateSomethingAsync(something);
            return Created(string.Empty, null);
        }
    }
}
