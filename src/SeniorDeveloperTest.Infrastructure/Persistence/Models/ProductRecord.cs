namespace SeniorDeveloperTest.Infrastructure.Persistence.Models;

public sealed record ProductRecord(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    DateTime CreatedDate,
    bool IsDeleted,
    DateTime? DeletedDate);