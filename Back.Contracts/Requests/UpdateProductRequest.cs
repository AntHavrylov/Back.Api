using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Contracts.Requests
{
    public class UpdateProductRequest
    {
        public string Name { get; init; }
        public string Description { get; init; }    
    }
}
