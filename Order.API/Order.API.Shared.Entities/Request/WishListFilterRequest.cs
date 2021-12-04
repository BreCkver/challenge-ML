
namespace Order.API.Shared.Entities.Request
{
    public class WishListFilterRequest
    {
        public int FilterType { get; set; }

        public UserDTO User { set; get; }
    }
}
