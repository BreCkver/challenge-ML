using Order.API.Business.Contracts;
using Order.API.Business.Contracts.Error;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Constants;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;
using System.Threading.Tasks;

namespace Order.API.Business.Person
{
    public class UserAuthenticateHandler : IPersonHandler<UserRequest, UserResponse>
    {
        private readonly IUserRepository repository;
        private UserDTO user;

        public UserAuthenticateHandler(IUserRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ResponseGeneric<UserResponse>> Execute(UserRequest request)
        {
            var userValidate = await IsValid(request);
            if (userValidate.Success)
            {
                var respose = new UserResponse { User = new UserDTO { UserName = user.UserName, Token = "" } };
                return ResponseGeneric.Create(respose);
            }
            return ResponseGeneric.CreateError<UserResponse>(userValidate.ErrorList);
        }

        public async Task<ResponseGeneric<bool>> IsValid(UserRequest request)
        {
            if (request == null || request.User == null)
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.REQUEST_NULL, ErrorMessage.REQUEST_NULL, ErrorType.BUSINESS));
            }

            if (ValidateRequest(request))
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.REQUEST_EMPTY, ErrorMessage.REQUEST_EMPTY, ErrorType.BUSINESS));
            }

            var UserExists = await repository.GetByUser(request.User);
            if (UserExists.Failure)
            {
                return UserExists.AsError<bool>();
            }
            if (UserExists.Value == null)
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.USERNAME_NOEXISTS, ErrorMessage.USERNAME_NOEXISTS, ErrorType.BUSINESS));
            }

            user = UserExists.Value;
            return ResponseGeneric.Create(true);
        }

        private bool ValidateRequest(UserRequest request) =>
            string.IsNullOrWhiteSpace(request.User.UserName) ||
                string.IsNullOrWhiteSpace(request.User.Password);
    }
}
