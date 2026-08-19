using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SeniorDeveloperTest.Application.Dtos;
using SeniorDeveloperTest.Application.Queries;
using SeniorDeveloperTest.Application.Services.Product.Dtos.Products;
using SeniorDeveloperTest.Application.Services.Product.Interfaces;

namespace SeniorDeveloperTest.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _productService.CreateAsync(
            request,
            cancellationToken);

        var version = HttpContext.GetRequestedApiVersion();

        return CreatedAtAction(
            nameof(GetById),
            new
            {
                version = version?.ToString(),
                id = product.Id
            },
            product);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(
            id,
            cancellationToken);

        return product is null
            ? NotFound()
            : Ok(product);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(PagedResult<ProductResponse>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged(
        [FromQuery] ProductQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _productService.GetPagedAsync(
            query,
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _productService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return product is null ? NotFound() : Ok(product);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var deleted = await _productService.DeleteAsync(
            id,
            cancellationToken);

        return deleted ? NoContent() : NotFound();
    }
    
    [HttpGet("{id:int}/price-conversion")]
    public async Task<IActionResult> ConvertPrice(
        int id,
        [FromQuery] string currency,
        CancellationToken cancellationToken)
    {
        var result = await _productService.ConvertPriceAsync(
            id,
            currency,
            cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}