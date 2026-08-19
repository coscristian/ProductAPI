namespace SeniorDeveloperTest.Infrastructure.ExternalServices.ExchangeRate.Models;

public sealed class ExchangeRateApiResponse
{
    public string Result { get; init; } = string.Empty;

    public string BaseCode { get; init; } = string.Empty;

    public Dictionary<string, decimal> Rates { get; init; } = new();
}