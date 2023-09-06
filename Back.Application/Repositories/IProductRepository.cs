using Back.Application.Models;

namespace Back.Application.Repositories;

public interface IProductRepository
{
    Task<bool> CreateAsync(Product product, CancellationToken token = default);
    
    Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default);
    
    Task<bool> UpdateAsync(Product product, CancellationToken token = default);
    
    Task<IEnumerable<Product>> GetAllProducts (GetAllProductsOptions options, 
        CancellationToken token = default);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

    Task<bool> ExistsById(Guid id, CancellationToken token = default);

    Task<bool> ExistsBySlug(string slug, CancellationToken token = default);    

    Task<int> GetCountAsync(string? name, CancellationToken token = default);
}
