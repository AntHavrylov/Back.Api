﻿using Back.Application.DataBase;
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
                """, product, cancellationToken: token));
            return result > 0;
        }

        public async Task<IEnumerable<Product>> GetAllProducts(GetAllProductsOptions options,
            CancellationToken token = default)
        {
            var orderClause = string.Empty;
            if (options.SortField is not null)
            {
                orderClause = $"""
                    order by {options.SortField} {(options.SortOrder == SortOrder.Ascending ? "asc" : "desc")}
                    """;
            }

            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            var result = await connection.QueryAsync(
                new CommandDefinition($"""
                    select * from {tableName}
                    where (@name is null or name like ('%' || @name || '%'))
                    {orderClause}
                    limit @pageSize
                    offset @pageOffset
                    """, new
                    {
                        name = options.Name,
                        pageOffset = (options.Page - 1) * options.PageSize,
                        pageSize = options.PageSize
                    },
                    cancellationToken: token));

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
                    """, new { id }, cancellationToken: token));
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

        public async Task<bool> ExustsBySlug(string slug, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            var result = await connection.ExecuteScalarAsync<bool>(new CommandDefinition($"""
                select count(1) from {tableName}
                where name like ('%' || @slug || '%')
                """, new { slug }, cancellationToken: token));
            return result;
        }

        public async Task<int> GetCountAsync(string? name, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            var result = await connection.QuerySingleAsync<int>(new CommandDefinition($"""
                select count(id) from {tableName}
                where (@name is null or name like('%' || @name || '%'))
                """, new { name }, cancellationToken: token));
            return (int)result;
        }
    }
}
