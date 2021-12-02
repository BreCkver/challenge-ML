using System.Threading.Tasks;
using Order.API.Business.Contracts;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Constants;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;
using Order.API.Shared.Framework.Helpers;
namespace Order.API.Business.Person
{
    public class UserCreatedHandler : IPersonHandler<UserCreatedRequest, UserResponse>
    {
        private readonly IUserRepository repository;

        public UserCreatedHandler(IUserRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ResponseGeneric<UserResponse>> Execute(UserCreatedRequest request)
        {
            var userValidate = await IsValid(request);
            if(userValidate.Success)
            {
                var newUserResponse = await repository.Insert(request.ConverToUserDTO());
                if(newUserResponse.Success)
                {
                    var newUser = newUserResponse.Value;
                    var respose = new UserResponse { User = new UserDTO { UserName = newUser.UserName, Token = "" } };
                    return ResponseGeneric.Create(respose);
                }
            }
            return ResponseGeneric.CreateError<UserResponse>(userValidate.ErrorList);
        }

        public async Task<ResponseGeneric<bool>> IsValid(UserCreatedRequest request)
        {
            if(request == null)
            {
                return ResponseGeneric.CreateError<bool>(new Error ("errorCode", "errorMessage", ErrorType.BUSINESS));
            }

            if(ValidateRequest(request))
            {
                return ResponseGeneric.CreateError<bool>(new Error("errorCode", "errorMessage", ErrorType.BUSINESS));
            }

            var UserExists = await repository.GetByUser(request);
            if(UserExists.Failure)
            {
                return UserExists.AsError<bool>();
            }
            
            if(UserExists.Value != null)
            {
                return ResponseGeneric.CreateError<bool>(new Error("errorCode", "errorMessage", ErrorType.BUSINESS));
            }

            //Pendiente validar, la contraseña sea la misma

            return ResponseGeneric.Create(true);
        }

        private bool ValidateRequest(UserCreatedRequest request) =>
            string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                    request.UserName.Length > 30 ||
                        request.Password.Length > 30;
    }
}
