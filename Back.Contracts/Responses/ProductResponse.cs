using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Contracts.Responses
{
    public class ProductResponse
    {
        public Guid Id { get; init; }

        public required string Name { get; init; }
        
        public required string Description { get; init; }   
    }
}
