namespace SeniorDeveloperTest.Application.Dtos.ExchangeRate;

public sealed record ProductPriceConversionResponse(
    int ProductId,
    string ProductName,
    decimal OriginalPrice,
    string OriginalCurrency,
    decimal ConvertedPrice,
    string TargetCurrency,
    decimal ExchangeRate);