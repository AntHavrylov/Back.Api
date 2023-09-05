using Back.Application.Models;
using Back.Application.Repositories;
using FluentValidation;

namespace Back.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<Product> _productValidator;

    public ProductService(IProductRepository productRepository, 
        IValidator<Product> productValidator)
    {
        _productRepository = productRepository;
        _productValidator = productValidator;
    }

    public async Task<bool> CreateAsync(Product product, 
        CancellationToken token = default)
    {
        await _productValidator.ValidateAndThrowAsync(product);
        return await _productRepository.CreateAsync(product,token);
    }
    
    public Task<IEnumerable<Product>> GetAllProducts(
        CancellationToken token = default)
    {
        return _productRepository.GetAllProducts(token);
    }

    public Task<Product?> GetByIdAsync(Guid id, 
        CancellationToken token = default)
    {
        return _productRepository.GetByIdAsync(id, token);
    }

    public async Task<Product?> UpdateAsync(Product product, 
        CancellationToken token = default)
    {
        await _productValidator.ValidateAndThrowAsync(product);
        var exists = await _productRepository.ExistsById(product.Id,token);
        if(!exists) 
        {
            return null;
        }
        await _productRepository.UpdateAsync(product, token);
        return product;
    }

    public Task<bool> DeleteByIdAsync(Guid id, 
        CancellationToken token = default)
    {
        return _productRepository.DeleteByIdAsync(id, token);
    }
}
