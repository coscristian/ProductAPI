using SeniorDeveloperTest.Application.Queries;

namespace SeniorDeveloperTest.Application.Services.Product.Interfaces;

public interface IProductRepository
{
    Task<Domain.Aggregates.ProductAggregate.Product> CreateAsync(Domain.Aggregates.ProductAggregate.Product product, CancellationToken cancellationToken = default);

    Task<Domain.Aggregates.ProductAggregate.Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<PagedResult<Domain.Aggregates.ProductAggregate.Product>> GetPagedAsync(ProductQuery query, CancellationToken cancellationToken = default);

    Task<Domain.Aggregates.ProductAggregate.Product?> UpdateAsync(Domain.Aggregates.ProductAggregate.Product product, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    
}