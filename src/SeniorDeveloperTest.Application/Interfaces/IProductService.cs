using SeniorDeveloperTest.Application.Dtos;
using SeniorDeveloperTest.Application.Queries;

namespace SeniorDeveloperTest.Application.Interfaces;

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
}