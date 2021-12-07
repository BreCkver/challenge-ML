using Order.API.Business.Contracts;
using Order.API.Business.Contracts.Error;
using Order.API.Business.Validations.Persons;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Constants;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;
using Order.API.Shared.Framework.Helpers;
using System.Threading.Tasks;

namespace Order.API.Business.Person
{
    public class UserAuthenticateHandler : UserBaseValidator, IPersonHandler<UserRequest, UserResponse>
    {
        private readonly IUserRepository repository;
        private UserDTO user;

        public UserAuthenticateHandler(IUserRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<ResponseGeneric<UserResponse>> Execute(UserRequest request)
        {
            var userValidate = await IsValid(request);
            if (userValidate.Success)
            {
                var respose = new UserResponse { User = new UserDTO { UserName = user.UserName, Identifier = user.Identifier, Token = "" } };
                return ResponseGeneric.Create(respose);
            }
            return ResponseGeneric.CreateError<UserResponse>(userValidate.ErrorList);
        }

        public async Task<ResponseGeneric<bool>> IsValid(UserRequest request)
        {
            var validationResult = await base.IsRequestValid(request);
            if (validationResult.Failure)
                return ResponseGeneric.CreateError<bool>(validationResult.ErrorList);
            return ResponseGeneric.Create(true);
        }

        protected override bool ValidateRequest(UserRequest request) =>
            string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Password);

        protected override async Task<ResponseGeneric<bool>> ValidateUser(UserRequest request)
        {
            var UserExists = await repository.GetByUser(request.ConverToUserDTO());
            if (UserExists.Failure)
            {
                return UserExists.AsError<bool>();
            }
            if (UserExists.Value == null || UserExists.Value.Identifier == null)
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.USERNAME_NOEXISTS, ErrorMessage.USERNAME_NOEXISTS, ErrorType.BUSINESS));
            }

            user = UserExists.Value;
            return ResponseGeneric.Create(true);
        }
    }
}
