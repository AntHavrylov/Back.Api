using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Contracts.Requests
{
    public class ProductRequest
    {
        public required Guid Guid { get; init; }

        public required string Name { get; init; }
        
        public required string Description { get; init; }       
    }
}
