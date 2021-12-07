using System.Threading.Tasks;
using Order.API.Business.Contracts;
using Order.API.Business.Validations.Persons;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;

namespace Order.API.UnitTest.Mocks
{
    public class UserHandlerMock : UserBaseValidator, IPersonHandler<UserRequest, UserResponse>
    {
        public UserHandlerMock(IUserRepository userRepository) : base(userRepository)
        {

        }

        public Task<ResponseGeneric<UserResponse>> Execute(UserRequest request)
        {
            return Task.FromResult(ResponseGeneric.Create(new UserResponse { User = request, }));
        }

        public async Task<ResponseGeneric<bool>> IsValid(UserRequest request)
        {
            var validationResult = await base.IsRequestValid(request);
            if (validationResult.Failure)
                return ResponseGeneric.CreateError<bool>(validationResult.ErrorList);
            return ResponseGeneric.Create(true);
        }

        protected override bool ValidateRequest(UserRequest request)
        => string.IsNullOrEmpty(request.UserName) || string.IsNullOrWhiteSpace(request.Password);
    }
}