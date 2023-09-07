using Back.Application.Models;
using Back.Application.Repositories;
using Back.Application.Services;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using System.Collections;

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
        _sut = new ProductService(_productRepository, _productValidator, _allProductsOptionsValidator);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnTrue_WhenProductCreated()
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
        var result = await _sut.CreateAsync(prod);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_WhenProductAllreadyExists()
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
        var result = await _sut.CreateAsync(prod);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnNonEmptyEnumerable_WhenProductsExist()
    {
        // Arrange
        var product = new Product()
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Description = "test"
        };

        _productRepository.GetAllAsync(Arg.Any<GetAllProductsOptions>()).Returns(
            new List<Product>()
            {
              product
            });

        // Act
        var result = await _sut.GetAllAsync(new GetAllProductsOptions());

        //Assert
        result.Should().ContainEquivalentOf(product);
        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAnEmptyEnumrable_WhenHaveNoProducts()
    {
        // Arrange
        _productRepository.GetAllAsync(Arg.Any<GetAllProductsOptions>())
            .Returns(Enumerable.Empty<Product>());

        // Act
        var result = await _sut.GetAllAsync(new GetAllProductsOptions());

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenExists()
    {
        // Arrange 
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Description = "test"
        };

        _productRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(product);

        // Act
        var result = await _sut.GetByIdAsync(product.Id);

        // Assert
        result.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNoProductExists()
    {
        // Arrange
        _productRepository.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

        // Act
        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenNoProductExists()
    {
        // Arrange
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Description = "test"
        };
        _productRepository.ExistsByIdAsync(Arg.Any<Guid>()).Returns(false);

        // Act
        var result = await _sut.UpdateAsync(product);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Description = "test"
        };
        _productRepository.ExistsByIdAsync(product.Id).Returns(true);
        _productRepository.UpdateAsync(product).Returns(true);

        // Act
        var result = await _sut.UpdateAsync(product);

        // Assert
        result.Should().BeEquivalentTo(product);
    }

}