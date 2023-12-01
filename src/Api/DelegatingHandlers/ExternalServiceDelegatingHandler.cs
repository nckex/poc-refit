using Microsoft.Extensions.Options;
using Api.Options;

namespace Api.DelegatingHandlers;

public class ExternalServiceDelegatingHandler : DelegatingHandler
{
    private readonly ExternalServiceApiOptions _externalServiceApiOptions;

    public ExternalServiceDelegatingHandler(
        IOptions<ExternalServiceApiOptions> options)
    {
        _externalServiceApiOptions = options?.Value
            ?? throw new ArgumentNullException(nameof(options));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("X-Api-Key", _externalServiceApiOptions.XApiKey);

        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        // Custom response handling here

        return response;
    }
}
