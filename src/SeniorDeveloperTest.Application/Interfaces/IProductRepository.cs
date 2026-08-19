using SeniorDeveloperTest.Application.Queries;
using SeniorDeveloperTest.Domain.Aggregates.ProductAggregate;

namespace SeniorDeveloperTest.Application.Interfaces;

public interface IProductRepository
{
    Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);

    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<PagedResult<Product>> GetPagedAsync(ProductQuery query, CancellationToken cancellationToken = default);

    Task<Product?> UpdateAsync(Product product, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    
}