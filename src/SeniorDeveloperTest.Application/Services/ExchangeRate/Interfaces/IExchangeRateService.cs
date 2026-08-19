namespace SeniorDeveloperTest.Application.Services.ExchangeRate.Interfaces;


public interface IExchangeRateService
{
    Task<decimal> GetExchangeRateAsync(
        string baseCurrency,
        string targetCurrency,
        CancellationToken cancellationToken = default);
}