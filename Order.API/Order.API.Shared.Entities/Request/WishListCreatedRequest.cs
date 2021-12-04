namespace Order.API.Shared.Entities.Request
{
    public class WishListCreatedRequest
    {
        public string Name { get; set; }

        public UserDTO User { set; get; }
    }
}
