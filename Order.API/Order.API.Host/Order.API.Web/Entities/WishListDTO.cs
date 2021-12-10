using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Order.API.Web.Entities
{
    public class WishListDTO : OrderDTO
    {
        public List<BookDTO> BookList { set; get; }
    }

    public class BookDTO : ProductDTO
    {
        public string Keyword { set; get; }
        public string Title { set; get; }
        public List<string> Authors { set; get; }
        public string Publisher { set; get; }

        public string Author
        {
            get
            {
                if (Authors != null)
                    return string.Join(",", Authors?.Select(a => a));
                else
                    return "";
            }
        }
    }

    public class ProductDTO
    {
        public int Identifier { set; get; }
        public string ExternalIdentifier { set; get; }
        public string Description { set; get; }
        public int Status { set; get; }
    }


}