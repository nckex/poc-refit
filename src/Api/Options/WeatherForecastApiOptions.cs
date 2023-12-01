namespace Api.Options;

public class WeatherForecastApiOptions
{
    public required string BaseAddress { get; set; }
    public required int RetryCount { get; set; }
    public required int RetryInterval { get; set; }
}
