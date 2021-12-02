using System;
using System.Collections.Generic;
using System.Text;

namespace Order.API.Shared.Entities.Response
{
    public class Meta
    {
        public Meta() { }
        public string Status { get; set; }
        public List<Message> Messages { get; set; }
    }
}
