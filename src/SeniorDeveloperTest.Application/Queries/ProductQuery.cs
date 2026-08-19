namespace SeniorDeveloperTest.Application.Queries;

public sealed record ProductQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? Search = null);