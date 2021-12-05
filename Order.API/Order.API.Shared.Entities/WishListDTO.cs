using Order.API.Shared.Entities.Parent;
using System.Collections.Generic;

namespace Order.API.Shared.Entities
{
    public class WishListDTO : OrderDTO
    {
        public List<BookDTO> BookList { set; get; }
    }
}
