using System.Data;
using Dapper;
using SeniorDeveloperTest.Application.Queries;
using SeniorDeveloperTest.Application.Services.Product.Interfaces;
using SeniorDeveloperTest.Domain.Aggregates.ProductAggregate;
using SeniorDeveloperTest.Infrastructure.Persistence;
using SeniorDeveloperTest.Infrastructure.Persistence.Models;

namespace SeniorDeveloperTest.Infrastructure.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public ProductRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Product> CreateAsync(
        Product product,
        CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();

        var command = new CommandDefinition(
            "sp_Product_Create",
            new
            {
                product.Name,
                product.Description,
                product.Price,
                product.CreatedDate
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        var record = await connection.QuerySingleAsync<ProductRecord>(command);

        return MapToDomain(record);
    }

    public async Task<Product?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();

        var command = new CommandDefinition(
            "sp_Product_GetById",
            new { Id = id },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        var record = await connection.QuerySingleOrDefaultAsync<ProductRecord>(command);

        return record is null
            ? null
            : MapToDomain(record);
    }

    public async Task<PagedResult<Product>> GetPagedAsync(
        ProductQuery query,
        CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();

        var command = new CommandDefinition(
            "sp_Product_GetPaged",
            new
            {
                query.PageNumber,
                query.PageSize,
                query.Search
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        using var multi = await connection.QueryMultipleAsync(command);

        var records = (await multi.ReadAsync<ProductRecord>()).AsList();
        var totalCount = await multi.ReadSingleAsync<int>();

        var products = records
            .Select(MapToDomain)
            .ToList();

        return new PagedResult<Product>(
            products,
            query.PageNumber,
            query.PageSize,
            totalCount);
    }

    public async Task<Product?> UpdateAsync(
        Product product,
        CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();

        var command = new CommandDefinition(
            "sp_Product_Update",
            new
            {
                product.Id,
                product.Name,
                product.Description,
                product.Price
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        var record = await connection.QuerySingleOrDefaultAsync<ProductRecord>(command);

        return record is null
            ? null
            : MapToDomain(record);
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();

        var command = new CommandDefinition(
            "sp_Product_Delete",
            new { Id = id },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        var rowsAffected = await connection.QuerySingleAsync<int>(command);

        return rowsAffected > 0;
    }

    private static Product MapToDomain(ProductRecord record)
    {
        return Product.Rehydrate(
            record.Id,
            record.Name,
            record.Description,
            record.Price,
            record.CreatedDate,
            record.IsDeleted,
            record.DeletedDate);
    }
}