using System.Threading.Tasks;
using Order.API.Business.Contracts;
using Order.API.Business.Contracts.Error;
using Order.API.Business.Validations.Persons;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Constants;
using Order.API.Shared.Entities.Enums;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;

namespace Order.API.Business.Person
{
    public class UserCreatedHandler : UserBaseValidator, IPersonHandler<UserRequest, UserResponse>
    {
        private readonly IUserRepository userRepository;

        public UserCreatedHandler(IUserRepository repository) : base(repository)
        {
            this.userRepository = repository;
        }

        public async Task<ResponseGeneric<UserResponse>> Execute(UserRequest request)
        {
            var userValidate = await IsValid(request);
            if(userValidate.Success)
            {
                var newUserResponse = await userRepository.Insert(request);
                if(newUserResponse.Success)
                {
                    var respose = new UserResponse { User = newUserResponse.Value };
                    return ResponseGeneric.Create(respose);
                }
                return ResponseGeneric.CreateError<UserResponse>(newUserResponse.ErrorList);
            }
            return ResponseGeneric.CreateError<UserResponse>(userValidate.ErrorList);
        }

        public async Task<ResponseGeneric<bool>> IsValid(UserRequest request)
        {
            var validationResult = await base.IsRequestValid(request);
            if (validationResult.Failure)
            { 
                return ResponseGeneric.CreateError<bool>(validationResult.ErrorList);
            }
            if (request.Password != request.PasswordConfirm)
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.USERNAME_EXISTS, ErrorMessage.USERNAME_EXISTS, ErrorType.BUSINESS));
            }
            return ResponseGeneric.Create(true);
        }

        protected override bool ValidateRequest(UserRequest request) =>
            string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                    string.IsNullOrWhiteSpace(request.PasswordConfirm) ||
                        request.UserName.Length > 100 ||
                           request.Password.Length > 100 ||
                              request.StatusIdentifier != (int)EnumUserStatus.New;
    }
}
