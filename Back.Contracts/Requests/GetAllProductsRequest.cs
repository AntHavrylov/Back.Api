namespace Back.Contracts.Requests;

public class GetAllProductsRequest 
{
    public required int Page { get; init; } = 1;
    
    public required int PageSize { get; init; } = 10;
    
    public required string? Name { get; init; }
    
    public required string? SortBy { get; init; }
}
