namespace Api.Options;

public record ExternalServiceApiOptions
{
    public required string BaseAddress { get; set; }
    public required string XApiKey { get; set; }
}
