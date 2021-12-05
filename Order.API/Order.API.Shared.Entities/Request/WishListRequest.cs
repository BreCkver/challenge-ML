using Order.API.Shared.Entities.Parent;

namespace Order.API.Shared.Entities.Request
{
    public class WishListRequest
    {
        public OrderDTO WishList { set; get; }
        public UserDTO User { set; get; }
    }
}
