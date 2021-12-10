using Order.API.Business.Contracts;
using Order.API.Business.Person;
using Order.API.Data;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Enums;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;
using Order.API.Shared.Framework.Helpers;

namespace Order.API.Host.Factory
{
    /// <summary>
    /// 
    /// </summary>
    public class UserFactory
    {
        private readonly string connectionString;
        /// <summary>
        /// 
        /// </summary>
        public UserFactory()
        {
            connectionString = Helper.GetConnection();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseGeneric<ICommandHandler<UserRequest,UserResponse>> Create(UserRequest request)
        {
            var data = new UserData(connectionString);           
            if(request.StatusIdentifier == (int)EnumUserStatus.New)
            {
                var handler = new UserCreatedHandler(data);
                return ResponseGeneric.Create((ICommandHandler<UserRequest, UserResponse>)handler);
            }
            else
            {
                var handler = new UserAuthenticateHandler(data);
                return ResponseGeneric.Create((ICommandHandler<UserRequest, UserResponse>)handler);
            }            
        }
    }
}