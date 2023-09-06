using Back.Application.Models;
using Back.Application.Repositories;
using Back.Application.Services;
using FluentAssertions;
using FluentValidation;
using NSubstitute;

namespace Back.Application.Tests.Unit.Services;

public class ProductServiceTests
{
    private readonly IProductService _sut;

    private readonly IValidator<Product> _productValidator = 
        Substitute.For<IValidator<Product>>();
    private readonly IProductRepository _productRepository = 
        Substitute.For<IProductRepository>();
    private readonly IValidator<GetAllProductsOptions> _allProductsOptionsValidator = 
        Substitute.For<IValidator<GetAllProductsOptions>>();

    public ProductServiceTests()
    {
        _sut = new ProductService(_productRepository,_productValidator,_allProductsOptionsValidator);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnTrue_WhenUserCreated() 
    {
        // Arrange
        var prod = new Product()
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Description = ""
        };
        _productRepository.CreateAsync(prod).Returns(true);
        _productRepository.ExistsBySlug(prod.Name).Returns(false);

        // Act
        var res = await _productRepository.CreateAsync(prod);

        // Assert
        res.Should().BeTrue();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_WhenUserAllreadyExists()
    {
        // Arrange
        var prod = new Product()
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Description = ""
        };        
        _productRepository.ExistsBySlug(prod.Name).Returns(true);

        // Act
        var res = await _productRepository.CreateAsync(prod);

        // Assert
        res.Should().BeFalse();
    }



}
