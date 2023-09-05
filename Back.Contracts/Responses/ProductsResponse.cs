using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Contracts.Responses
{
    public class ProductsResponse
    {
        public required IEnumerable<ProductResponse> Items { get; init; } = Enumerable.Empty<ProductResponse>();
    }
}
