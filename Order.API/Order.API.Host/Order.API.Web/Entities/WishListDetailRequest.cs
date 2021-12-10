using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Order.API.Web.Entities
{
    public class WishListDetailRequest
    {
        public WishListDTO WishList { set; get; }
        public UserDTO User { set; get; }
    }
}