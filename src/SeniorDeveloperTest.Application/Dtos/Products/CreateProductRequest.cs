namespace SeniorDeveloperTest.Application.Dtos;

public sealed record CreateProductRequest(string Name, string? Description, decimal Price);