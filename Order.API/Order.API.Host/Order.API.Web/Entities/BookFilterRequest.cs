using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Order.API.Web.Entities
{
    public class BookFilterRequest
    {
        public BookDTO Book { set; get; }
        public UserDTO User { set; get; }
    }
}