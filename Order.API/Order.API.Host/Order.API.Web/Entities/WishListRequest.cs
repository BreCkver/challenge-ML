using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Order.API.Web.Entities
{
    public class WishListRequest
    {
        public OrderDTO WishList { set; get; }
        public UserDTO User { set; get; }
    }
}