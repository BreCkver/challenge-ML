using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Order.API.Web.Entities
{
    public class OrderDTO
    {
        public long Identifier { set; get; }

        public int Status { set; get; }

        public string Name { set; get; }
    }
}