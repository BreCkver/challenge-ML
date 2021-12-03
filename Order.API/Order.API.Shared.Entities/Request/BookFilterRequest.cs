namespace Order.API.Shared.Entities.Request
{
    public class BookFilterRequest
    {
        public BookDTO Book { set; get; }

        public UserDTO User { set; get; }
    }
}
