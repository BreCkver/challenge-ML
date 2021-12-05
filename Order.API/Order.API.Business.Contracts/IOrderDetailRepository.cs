using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Parent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Order.API.Business.Contracts
{
    public interface IOrderDetailRepository 
    {
        Task<ResponseGeneric<IEnumerable<BookDTO>>> GetAllByOrder(OrderDTO order);

        Task<ResponseGeneric<bool>> Add(OrderDTO order, IEnumerable<BookDTO> bookList);

        Task<ResponseGeneric<bool>> Update(OrderDTO order, IEnumerable<BookDTO> bookList);
    }
}
