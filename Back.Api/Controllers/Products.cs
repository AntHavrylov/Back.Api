using Back.Api.Mapping;
using Back.Application.Services;
using Back.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back.Api.Controllers;

[ApiController]
public class Products : ControllerBase
{
    private readonly IProductService _productService;

    public Products(IProductService productService)
    {
        _productService = productService;
    }

    [Authorize(AuthConstants.TrustedMemberPolicyName)]    
    [HttpPost(ApiEndpoints.Products.Create)]
    public async Task<IActionResult> GetAsync([FromBody] CreateProductRequest createProductRequest, 
        CancellationToken token)
    {
        var product = createProductRequest.MapToProduct();
        var result = await _productService.CreateAsync(product, token);
        if (!result) 
        {
            return Conflict();
        }
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product.MapToProductResponse());
    }

    [Authorize]
    [HttpGet(ApiEndpoints.Products.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id, 
        CancellationToken token)
    {
        var product = await _productService.GetByIdAsync(id, token);
        return product is null ? NotFound() : Ok(product.MapToProductResponse());
    }

    [Authorize]
    [HttpGet(ApiEndpoints.Products.GetAll)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllProductsRequest request,
        CancellationToken token)
    {
        var options = request.MapToOptions();
        var propductCount = await _productService.GetCountAsync(options.Name, token);


        var products = await _productService.GetAllAsync(options, token);
        return Ok(products.MapToProductResponses(request.Page,request.PageSize, propductCount));
    }

    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpPut(ApiEndpoints.Products.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, 
        [FromBody] UpdateProductRequest updateProductRequest,
        CancellationToken token)
    {
        var product = updateProductRequest.MapToProduct(id);
        var updated = await _productService.UpdateAsync(product, token);
        if (updated is null)
        {
            return NotFound();
        }
        return Ok(updated.MapToProductResponse());
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete(ApiEndpoints.Products.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id,
        CancellationToken token)
    {
        var deleted = await _productService.DeleteByIdAsync(id, token);
        if (!deleted)
        {
            return NotFound();
        }
        return Ok();
    }
}
