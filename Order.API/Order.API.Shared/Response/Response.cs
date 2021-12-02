using System;
using System.Collections.Generic;
using System.Text;

namespace Order.API.Shared.Response
{
    public class Response<T>
    {
        public Response() { }
        public Meta Meta { get; set; }
        public T Data { get; set; }

    }
}
