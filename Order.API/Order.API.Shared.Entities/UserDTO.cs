
namespace Order.API.Shared.Entities
{
    public class UserDTO
    {
        public int Identifier { set; get; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
