namespace SeniorDeveloperTest.Application.Queries;

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount)
{
    public int TotalPages =>
        (int)Math.Ceiling((double)TotalCount / PageSize);
}