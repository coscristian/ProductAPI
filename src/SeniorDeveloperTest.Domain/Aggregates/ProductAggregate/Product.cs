namespace SeniorDeveloperTest.Domain.Aggregates.ProductAggregate;


public sealed class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedDate { get; private set; }

    private Product(
        int id,
        string name,
        string? description,
        decimal price,
        DateTime createdDate,
        bool isDeleted,
        DateTime? deletedDate)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        CreatedDate = createdDate;
        IsDeleted = isDeleted;
        DeletedDate = deletedDate;
    }

    public static Product Create(
        string name,
        string? description,
        decimal price)
    {
        Validate(name, price);

        return new Product(
            0,
            name.Trim(),
            description?.Trim(),
            price,
            DateTime.UtcNow,
            false,
            null);
    }

    public static Product Rehydrate(
        int id,
        string name,
        string? description,
        decimal price,
        DateTime createdDate,
        bool isDeleted,
        DateTime? deletedDate)
    {
        Validate(name, price);

        return new Product(
            id,
            name,
            description,
            price,
            createdDate,
            isDeleted,
            deletedDate);
    }

    public void Update(
        string name,
        string? description,
        decimal price)
    {
        if (IsDeleted)
            throw new InvalidOperationException("A deleted product cannot be updated.");

        Validate(name, price);

        Name = name.Trim();
        Description = description?.Trim();
        Price = price;
    }

    public void Delete()
    {
        if (IsDeleted)
            return;

        IsDeleted = true;
        DeletedDate = DateTime.UtcNow;
    }

    private static void Validate(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Product name is required.",
                nameof(name));

        if (name.Length > 200)
            throw new ArgumentException(
                "Product name cannot exceed 200 characters.",
                nameof(name));

        if (price <= 0)
            throw new ArgumentException(
                "Product price must be greater than zero.",
                nameof(price));
    }
}