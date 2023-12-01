using Refit;
using Polly;
using Polly.Timeout;
using Polly.Extensions.Http;
using Microsoft.Extensions.Options;
using Api.Options;
using Api.DelegatingHandlers;
using Infra.Clients.WeatherForecastApi;
using Infra.Clients.ExternalServiceApi;

namespace Api.Extensions;

public static class InfraServiceCollectionExtensions
{
    public static void AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        AddWeatherForecastClient(services, configuration);
        AddExternalServiceClient(services, configuration);
    }

    private static void AddWeatherForecastClient(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<WeatherForecastApiOptions>(configuration.GetSection("WeatherForecastApi"));

        services
            .AddRefitClient<IWeatherForecastApiClient>()
            .ConfigureHttpClient((provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<WeatherForecastApiOptions>>();
                client.BaseAddress = new Uri(options.Value.BaseAddress);
            })
            .AddPolicyHandler((provider, _) =>
            {
                var options = provider.GetRequiredService<IOptions<WeatherForecastApiOptions>>();

                var policy = HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .Or<TimeoutRejectedException>()
                    .WaitAndRetryAsync(options.Value.RetryCount, _ => TimeSpan.FromMilliseconds(options.Value.RetryInterval));

                return policy;
            });
    }

    private static void AddExternalServiceClient(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ExternalServiceApiOptions>(configuration.GetSection("ExternalServiceApi"));

        services.AddTransient<ExternalServiceDelegatingHandler>();

        services
            .AddRefitClient<IExternalServiceApiClient>()
            .ConfigureHttpClient((provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<ExternalServiceApiOptions>>();
                client.BaseAddress = new Uri(options.Value.BaseAddress);
            })
            .AddHttpMessageHandler<ExternalServiceDelegatingHandler>();
    }
}
