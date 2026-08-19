using SeniorDeveloperTest.Application.Dtos;
using SeniorDeveloperTest.Application.Dtos.ExchangeRate;
using SeniorDeveloperTest.Application.Queries;
using SeniorDeveloperTest.Application.Services.Product.Dtos.Products;

namespace SeniorDeveloperTest.Application.Services.Product.Interfaces;

public interface IProductService
{
    Task<ProductResponse> CreateAsync(
        CreateProductRequest request,
        CancellationToken cancellationToken = default);

    Task<ProductResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<PagedResult<ProductResponse>> GetPagedAsync(
        ProductQuery query,
        CancellationToken cancellationToken = default);

    Task<ProductResponse?> UpdateAsync(
        int id,
        UpdateProductRequest request,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
    
    Task<ProductPriceConversionResponse?> ConvertPriceAsync(
        int productId,
        string targetCurrency,
        CancellationToken cancellationToken = default);
}