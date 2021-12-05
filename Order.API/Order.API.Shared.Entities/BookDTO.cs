using Order.API.Shared.Entities.Parent;
using System.Collections.Generic;

namespace Order.API.Shared.Entities
{
    public class BookDTO : ProductDTO
    {       
        public string Keyword { set; get; }
        public string Title { set; get; }
        public List<string> Authors { set; get; }
        public string Publisher { set; get; }
    }
}
