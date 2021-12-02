using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Request;

namespace Order.API.Shared.Framework.Helpers
{
    public static class HelperConvert
    {
        public static UserDTO ConverToUserDTO(this UserCreatedRequest request)
        {
            return new UserDTO
            {
                UserName = request.UserName,
                Password = request.Password,
            };
        }
    }
}
