using Order.API.Business.Contracts;
using Order.API.Business.Person;
using Order.API.Data;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;

namespace Order.API.Host.Factory
{
    public class UserFactory
    {
        private readonly string connectionString = "";

        public ResponseGeneric<IPersonHandler<UserRequest,UserResponse>> Create(UserRequest request)
        {
            var data = new UserData(connectionString);
            var handler =  new UserCreatedHandler(data);
            return ResponseGeneric.Create((IPersonHandler<UserRequest, UserResponse>)handler);
        }
    }
}