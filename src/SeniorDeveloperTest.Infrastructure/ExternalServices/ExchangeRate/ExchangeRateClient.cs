using System.Net;
using System.Net.Http.Json;
using SeniorDeveloperTest.Application.Services.ExchangeRate.Interfaces;
using SeniorDeveloperTest.Infrastructure.ExternalServices.ExchangeRate.Models;

namespace SeniorDeveloperTest.Infrastructure.ExternalServices.ExchangeRate;


public sealed class ExchangeRateClient : IExchangeRateService
{
    private readonly HttpClient _httpClient;

    public ExchangeRateClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal> GetExchangeRateAsync(
        string baseCurrency,
        string targetCurrency,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            $"latest/{baseCurrency.ToUpperInvariant()}",
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ExchangeRateApiResponse>(
            cancellationToken);

        if (result is null ||
            !result.Rates.TryGetValue(
                targetCurrency.ToUpperInvariant(),
                out var rate))
        {
            throw new InvalidOperationException(
                $"Exchange rate not found for {baseCurrency} to {targetCurrency}.");
        }

        return rate;
    }
}