using Domain.Models;
using Refit;

namespace Infra.Clients.WeatherForecastApi;

public interface IWeatherForecastApiClient
{
    [Get("/WeatherForecast")]
    Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync();

    [Get("/WeatherForecast/Broken")]
    Task GetBrokenAsync();
}
