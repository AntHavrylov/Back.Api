using Back.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Application.Services;

public interface IProductService
{
    Task<bool> CreateAsync(Product product, CancellationToken token = default);

    Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default);

    Task<Product?> UpdateAsync(Product product, CancellationToken token = default);

    Task<IEnumerable<Product>> GetAllProducts(GetAllProductsOptions options, CancellationToken token = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

    Task<int> GetCountAsync(string? name, CancellationToken token = default);
}
