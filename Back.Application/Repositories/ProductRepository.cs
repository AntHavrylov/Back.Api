using Back.Application.DataBase;
using Back.Application.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Application.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private const string tableName = "products";
        private readonly IDbConnectionFactory _dbConnectionFactory;


        public ProductRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<bool> CreateAsync(Product product, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            var result = await connection.ExecuteAsync(                
                new CommandDefinition("""
                insert into products (id,name,description)
                values(@Id,@Name,@Description)
                """, product , cancellationToken: token));
            return result > 0;
        }

        public async Task<IEnumerable<Product>> GetAllProducts(CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            var result = await connection.QueryAsync(
                new CommandDefinition($"""
                    select * from {tableName}
                    """,cancellationToken: token));
            return result.Select(p => new Product()
            {
                Id = p.id,
                Name = p.name,
                Description = p.description,
            });
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            var result = await connection.QuerySingleOrDefaultAsync<Product>(
                new CommandDefinition($"""
                    select * from {tableName}
                    where id = @id
                    """, new { id },  cancellationToken: token));
            return result;
        }

        public async Task<bool> UpdateAsync(Product product, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            await connection.ExecuteAsync(
                new CommandDefinition($"""
                    delete from {tableName} 
                    where id = @id
                """, new { id = product.Id }, cancellationToken: token));

            var result = await connection.ExecuteAsync(
                new CommandDefinition($"""
                    insert into {tableName} (id, name, description)
                    values (@Id,@Name,@Description)
                    """, product, cancellationToken: token));

            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            var result = await connection.ExecuteAsync(new CommandDefinition($"""
                delete from {tableName}
                where id = @id
                """, new { id }, cancellationToken: token));
            return result > 0;
        }

        public async Task<bool> ExistsById(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            var result = await connection.ExecuteScalarAsync<bool>(new CommandDefinition($"""
                select count(1) from {tableName} 
                where id = @id  
                """, new { id }, cancellationToken: token));
            return result;
        }
    }
}
