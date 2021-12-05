using Order.API.Business.Contracts;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Parent;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Order.API.Data
{
    public class OrderData : IOrderRepository
    {
        public Task<ResponseGeneric<IEnumerable<OrderDTO>>> GetAllByUser(int userIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseGeneric<OrderDTO>> GetOrder(OrderDTO order, int userIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseGeneric<OrderDTO>> Insert(OrderDTO order, int userIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseGeneric<bool>> Update(OrderDTO order)
        {
            throw new NotImplementedException();
        }
    }
}
