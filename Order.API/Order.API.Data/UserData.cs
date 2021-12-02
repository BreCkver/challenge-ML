using Order.API.Business.Contracts;
using Order.API.Shared.Entities;
using System;
using System.Threading.Tasks;

namespace Order.API.Data
{
    public class UserData : IUserRepository
    {
        private readonly string connectionString;

        public UserData(string connectionString) => this.connectionString = connectionString;
        public Task<ResponseGeneric<UserDTO>> GetByUser(UserDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseGeneric<UserDTO>> Insert(UserDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
