using Back.Application.Models;
using Back.Contracts.Requests;
using Back.Contracts.Responses;

namespace Back.Api.Mapping
{
    public static class ContractMapping
    {
        public static Product MapToProduct(this CreateProductRequest createProductRequest) =>
            new()
            {
                Id = Guid.NewGuid(),
                Name = createProductRequest.Name,
                Description = createProductRequest.Description
            };

        public static Product MapToProduct(this UpdateProductRequest updateProductRequest, Guid id) =>
            new()
            {
                Id = id,
                Name = updateProductRequest.Name,
                Description = updateProductRequest.Description,
            };

        public static ProductResponse MapToProductResponse(this Product product) =>
            new()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
            };

        public static ProductsResponse MapToProductResponses(
            this IEnumerable<Product> products,
            int page,
            int pageSize,
            int productCount
            ) =>
            new()
            {
                Items = products.Select(MapToProductResponse),
                Page = page,
                PageSize = pageSize,
                Total = productCount
            };

        public static GetAllProductsOptions MapToOptions(this GetAllProductsRequest request) =>
            new()
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Name = request.Name,
                SortField = request.SortBy?.Trim('+','-'), 
                SortOrder = request.SortBy is null ? 
                    SortOrder.Unsorted : 
                    request.SortBy.StartsWith('-') ? 
                        SortOrder.Descending:
                        SortOrder.Ascending,
            };
                
    }
}
