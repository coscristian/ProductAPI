using SeniorDeveloperTest.Application.Dtos;
using SeniorDeveloperTest.Application.Dtos.ExchangeRate;
using SeniorDeveloperTest.Application.Queries;
using SeniorDeveloperTest.Application.Services.ExchangeRate.Interfaces;
using SeniorDeveloperTest.Application.Services.Product.Dtos.Products;
using SeniorDeveloperTest.Application.Services.Product.Interfaces;
using DomainProduct = SeniorDeveloperTest.Domain.Aggregates.ProductAggregate.Product;

namespace SeniorDeveloperTest.Application.Services.Product;

public sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IExchangeRateService _exchangeRateService;

    public ProductService(IProductRepository productRepository, IExchangeRateService exchangeRateService)
    {
        _productRepository = productRepository;
        _exchangeRateService = exchangeRateService;
    }

    public async Task<ProductResponse> CreateAsync(
        CreateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var product = DomainProduct.Create(
            request.Name,
            request.Description,
            request.Price);

        var createdProduct = await _productRepository.CreateAsync(
            product,
            cancellationToken);

        return MapToResponse(createdProduct);
    }

    public async Task<ProductResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(
            id,
            cancellationToken);

        return product is null
            ? null
            : MapToResponse(product);
    }

    public async Task<PagedResult<ProductResponse>> GetPagedAsync(
        ProductQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _productRepository.GetPagedAsync(
            query,
            cancellationToken);

        var products = result.Items
            .Select(MapToResponse)
            .ToList();

        return new PagedResult<ProductResponse>(
            products,
            result.PageNumber,
            result.PageSize,
            result.TotalCount);
    }

    public async Task<ProductResponse?> UpdateAsync(
        int id,
        UpdateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (product is null)
            return null;

        product.Update(
            request.Name,
            request.Description,
            request.Price);

        var updatedProduct = await _productRepository.UpdateAsync(
            product,
            cancellationToken);

        return updatedProduct is null
            ? null
            : MapToResponse(updatedProduct);
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _productRepository.DeleteAsync(
            id,
            cancellationToken);
    }

    public async Task<ProductPriceConversionResponse?> ConvertPriceAsync(
        int productId,
        string targetCurrency,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(
            productId,
            cancellationToken);

        if (product is null)
            return null;

        const string baseCurrency = "COP";

        if (targetCurrency.Equals(
                baseCurrency,
                StringComparison.OrdinalIgnoreCase))
        {
            return new ProductPriceConversionResponse(
                product.Id,
                product.Name,
                product.Price,
                baseCurrency,
                product.Price,
                baseCurrency,
                1);
        }

        var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(
            baseCurrency,
            targetCurrency,
            cancellationToken);

        var convertedPrice = product.Price * exchangeRate;

        return new ProductPriceConversionResponse(
            product.Id,
            product.Name,
            product.Price,
            baseCurrency,
            convertedPrice,
            targetCurrency.ToUpperInvariant(),
            exchangeRate);
    }

    private static ProductResponse MapToResponse(DomainProduct product)
    {
        return new ProductResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.CreatedDate);
    }
}