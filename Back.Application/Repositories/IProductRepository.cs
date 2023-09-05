using Back.Application.Models;

namespace Back.Application.Repositories;

public interface IProductRepository
{
    Task<bool> CreateAsync(Product product, CancellationToken token = default);
    
    Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default);
    
    Task<bool> UpdateAsync(Product product, CancellationToken token = default);
    
    Task<IEnumerable<Product>> GetAllProducts (CancellationToken token = default);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

    Task<bool> ExistsById(Guid id, CancellationToken token = default);

}
