using SeniorDeveloperTest.Application.Dtos;
using SeniorDeveloperTest.Application.Interfaces;
using SeniorDeveloperTest.Application.Queries;
using SeniorDeveloperTest.Domain.Aggregates.ProductAggregate;

namespace SeniorDeveloperTest.Application.Services;

public sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponse> CreateAsync(
        CreateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var product = Product.Create(
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

    private static ProductResponse MapToResponse(Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.CreatedDate);
    }
}