using Moq;
using SeniorDeveloperTest.Application.Services.ExchangeRate.Interfaces;
using SeniorDeveloperTest.Application.Services.Product;
using SeniorDeveloperTest.Application.Services.Product.Dtos.Products;
using SeniorDeveloperTest.Application.Services.Product.Interfaces;
using DomainProduct = SeniorDeveloperTest.Domain.Aggregates.ProductAggregate.Product;

namespace SeniorDeveloperTest.Tests.Services.Product;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IExchangeRateService> _exchangeRateServiceMock;
    private readonly ProductService _sut;

    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _exchangeRateServiceMock = new Mock<IExchangeRateService>();

        _sut = new ProductService(
            _productRepositoryMock.Object,
            _exchangeRateServiceMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateProductSuccessfully()
    {
        // Arrange
        var request = new CreateProductRequest(
            "Mechanical Keyboard",
            "RGB mechanical keyboard",
            120000);

        var createdProduct = DomainProduct.Create(
            request.Name,
            request.Description,
            request.Price);

        _productRepositoryMock
            .Setup(x => x.CreateAsync(
                It.IsAny<DomainProduct>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdProduct);

        // Act
        var result = await _sut.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Mechanical Keyboard", result.Name);
        Assert.Equal("RGB mechanical keyboard", result.Description);
        Assert.Equal(120000m, result.Price);

        _productRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<DomainProduct>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var product = DomainProduct.Create(
            "Wireless Mouse",
            "Wireless mouse",
            50000);

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        var result = await _sut.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Wireless Mouse", result.Name);
        Assert.Equal("Wireless mouse", result.Description);
        Assert.Equal(50000m, result.Price);

        _productRepositoryMock.Verify(
            x => x.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(
                999,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((DomainProduct?)null);

        // Act
        var result = await _sut.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenProductIsDeleted()
    {
        // Arrange
        _productRepositoryMock
            .Setup(x => x.DeleteAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _sut.DeleteAsync(1);

        // Assert
        Assert.True(result);

        _productRepositoryMock.Verify(
            x => x.DeleteAsync(
                1,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task ConvertPriceAsync_ShouldConvertPrice_WhenProductExists()
    {
        // Arrange
        var product = DomainProduct.Create(
            "Mechanical Keyboard",
            "RGB mechanical keyboard",
            120000);

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _exchangeRateServiceMock
            .Setup(x => x.GetExchangeRateAsync(
                "COP",
                "USD",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(0.00025m);

        // Act
        var result = await _sut.ConvertPriceAsync(1, "USD");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Mechanical Keyboard", result.ProductName);
        Assert.Equal(120000m, result.OriginalPrice);
        Assert.Equal("COP", result.OriginalCurrency);
        Assert.Equal(30m, result.ConvertedPrice);
        Assert.Equal("USD", result.TargetCurrency);
        Assert.Equal(0.00025m, result.ExchangeRate);

        _exchangeRateServiceMock.Verify(
            x => x.GetExchangeRateAsync(
                "COP",
                "USD",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task ConvertPriceAsync_ShouldReturnOriginalPrice_WhenTargetCurrencyIsCop()
    {
        // Arrange
        var product = DomainProduct.Create(
            "Mechanical Keyboard",
            "RGB mechanical keyboard",
            120000);

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        var result = await _sut.ConvertPriceAsync(1, "COP");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Mechanical Keyboard", result.ProductName);
        Assert.Equal(120000m, result.OriginalPrice);
        Assert.Equal(120000m, result.ConvertedPrice);
        Assert.Equal("COP", result.OriginalCurrency);
        Assert.Equal("COP", result.TargetCurrency);
        Assert.Equal(1m, result.ExchangeRate);

        _exchangeRateServiceMock.Verify(
            x => x.GetExchangeRateAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}