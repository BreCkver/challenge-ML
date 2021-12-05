using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.API.Shared.Entities.Parent
{
    public class ProductDTO
    {
        public int Identifier { set; get; }
        public string ExternalIdentifier { set; get; }
        public string Description { set; get; }
    }
}
