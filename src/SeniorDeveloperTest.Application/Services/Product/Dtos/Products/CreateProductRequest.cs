namespace SeniorDeveloperTest.Application.Services.Product.Dtos.Products;

public sealed record CreateProductRequest(string Name, string? Description, decimal Price);