using Back.Application.Models;
using Back.Application.Repositories;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Back.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<Product> _productValidator;
    private readonly IValidator<GetAllProductsOptions> _allProductsOptionsValidator;

    public ProductService(IProductRepository productRepository,
        IValidator<Product> productValidator,
        IValidator<GetAllProductsOptions> allProductsOptionsValidator)
    {
        _productRepository = productRepository;
        _productValidator = productValidator;
        _allProductsOptionsValidator = allProductsOptionsValidator;
    }

    public async Task<bool> CreateAsync(Product product,
        CancellationToken token = default)
    {
        var exists = await _productRepository.ExistsBySlug(product.Name, token);
        if (exists)
        {
            return false;
        }
        await _productValidator.ValidateAndThrowAsync(product);
        return await _productRepository.CreateAsync(product, token);
    }

    public async Task<IEnumerable<Product>> GetAllProducts(
        GetAllProductsOptions options,
        CancellationToken token = default)
    {
        await _allProductsOptionsValidator.ValidateAndThrowAsync(options);
        var products = await _productRepository.GetAllProducts(options, token);
        return products;
    }

    public async Task<Product?> GetByIdAsync(Guid id,
        CancellationToken token = default)
    {
        return await _productRepository.GetByIdAsync(id, token);
    }

    public async Task<Product?> UpdateAsync(Product product,
        CancellationToken token = default)
    {
        await _productValidator.ValidateAndThrowAsync(product);
        var exists = await _productRepository.ExistsById(product.Id, token);
        if (!exists)
        {
            return null;
        }
        await _productRepository.UpdateAsync(product, token);
        return product;
    }

    public async Task<bool> DeleteByIdAsync(Guid id,
        CancellationToken token = default)
    {
        return await _productRepository.DeleteByIdAsync(id, token);
    }

    public async Task<int> GetCountAsync(string? name, CancellationToken token = default)
    {
        return await _productRepository.GetCountAsync(name, token);
    }
}
