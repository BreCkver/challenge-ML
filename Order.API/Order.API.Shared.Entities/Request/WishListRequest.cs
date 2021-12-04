namespace Order.API.Shared.Entities.Request
{
    public class WishListRequest
    {
        public WishListDTO WishList { set; get; }
        public UserDTO User { set; get; }
    }
}
