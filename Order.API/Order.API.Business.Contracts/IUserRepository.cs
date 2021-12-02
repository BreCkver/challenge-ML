using Order.API.Shared.Entities;
using System.Threading.Tasks;

namespace Order.API.Business.Contracts
{
    public interface IUserRepository
    {
        Task<ResponseGeneric<UserDTO>> GetByUser(UserDTO entity);

        Task<ResponseGeneric<UserDTO>> Insert(UserDTO entity);

    }
}
