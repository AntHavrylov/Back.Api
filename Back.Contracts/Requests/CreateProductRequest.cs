﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Contracts.Requests
{
    public class CreateProductRequest
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
    }
}
