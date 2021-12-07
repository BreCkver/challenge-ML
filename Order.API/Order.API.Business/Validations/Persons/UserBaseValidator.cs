using Order.API.Business.Contracts;
using Order.API.Business.Contracts.Error;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Constants;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Framework.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.Business.Validations.Persons
{
    public abstract class UserBaseValidator : IValidator<UserRequest>
    {
        private readonly IUserRepository userRepository;

        public UserBaseValidator(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<ResponseGeneric<bool>> IsRequestValid(UserRequest request)
        {
            if (request == null)
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.REQUEST_NULL, ErrorMessage.REQUEST_NULL, ErrorType.BUSINESS));
            }
            if (ValidateRequest(request))
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.REQUEST_EMPTY, ErrorMessage.REQUEST_EMPTY, ErrorType.BUSINESS));
            }

            if(!request.UserName.ValidateCharacters() || !request.Password.ValidateCharacters())
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.REQUEST_NOALPHANUMERIC, ErrorMessage.REQUEST_NOALPHANUMERIC, ErrorType.BUSINESS));
            }

            var validateUserResult = await ValidateUser(request);
            if (validateUserResult.Failure)
            {
                return validateUserResult.AsError<bool>();
            }

            return ResponseGeneric.Create(true);
        }

        protected abstract bool ValidateRequest(UserRequest request);

        protected virtual async Task<ResponseGeneric<bool>> ValidateUser(UserRequest request)
        {
            var UserExists = await userRepository.GetByUser(request);
            if (UserExists.Failure)
            {
                return UserExists.AsError<bool>();
            }
            if (UserExists.Value != null && UserExists.Value.Identifier != null)
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.USERNAME_EXISTS, ErrorMessage.USERNAME_EXISTS, ErrorType.BUSINESS));
            }
            return ResponseGeneric.Create(true);
        }
    }
}
