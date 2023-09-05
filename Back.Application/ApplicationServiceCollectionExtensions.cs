using Back.Application.DataBase;
using Back.Application.Repositories;
using Back.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Back.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IProductService, ProductService>();
        services.AddSingleton<IProductRepository, ProductRepository>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString) 
    {
        services.AddSingleton<IDbConnectionFactory>(_ 
            => new NpgsqlConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
}
