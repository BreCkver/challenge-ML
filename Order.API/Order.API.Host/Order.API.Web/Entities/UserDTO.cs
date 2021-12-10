using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Order.API.Web.Entities
{
    public class UserDTO
    {
        public int? Identifier { set; get; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string PasswordConfirm { get; set; }
        public int StatusIdentifier { set; get; }
    }
}