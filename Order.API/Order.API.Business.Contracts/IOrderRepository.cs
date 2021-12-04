using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Parent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Order.API.Business.Contracts
{
    public interface IOrderRepository
    {
        Task<ResponseGeneric<IEnumerable<OrderDTO>>> GetAllByUser(int userIdentifier);

        Task<ResponseGeneric<OrderDTO>> Insert(OrderDTO order, int userIdentifier);

        Task<ResponseGeneric<bool>> Update(OrderDTO order);

        Task<ResponseGeneric<OrderDTO>> GetOrder(OrderDTO order, int userIdentifier);
    }
}
