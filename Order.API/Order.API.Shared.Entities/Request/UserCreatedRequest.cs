namespace Order.API.Shared.Entities.Request
{
    public class UserCreatedRequest : UserDTO
    {
        public string PasswordConfirm { get; set; }
    }
}
