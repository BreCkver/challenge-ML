using System.Threading.Tasks;
using Order.API.Business.Contracts;
using Order.API.Business.Validations.Orders;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;
using Order.API.Shared.Framework.Helpers;

namespace Order.API.Business.Orders
{
    public class WishListUpdatedHandler : OrderBaseValidator, ICommandHandler<WishListRequest, WishListResponse>
    {
        private readonly IOrderRepository orderRepository;

        public WishListUpdatedHandler(IUserRepository userRepository, IOrderRepository orderRepository) : base(userRepository, orderRepository)
        {
            this.orderRepository = orderRepository;
        }
        public async Task<ResponseGeneric<WishListResponse>> Execute(WishListRequest request)
        {
            var orderValidated = await IsValid(request);
            if (orderValidated.Success)
            {
                var orderInsertResult = await orderRepository.Update(request.ConverToOrderDTO());
                if (orderInsertResult.Success)
                {
                    var respose = new WishListResponse { WishList = request.WishList };
                    return ResponseGeneric.Create(respose);
                }
                return orderInsertResult.AsError<WishListResponse>();
            }
            return ResponseGeneric.CreateError<WishListResponse>(orderValidated.ErrorList);
        }

        public async Task<ResponseGeneric<bool>> IsValid(WishListRequest request)
        {
            var validationResult = await base.IsRequestValid(request);
            if (validationResult.Failure)
                return ResponseGeneric.CreateError<bool>(validationResult.ErrorList);
            return ResponseGeneric.Create(true);
        }

        protected override bool ValidateRequest(WishListRequest request) =>
           request.User.Identifier == null ||
            request.WishList.Identifier == default ||
                request.WishList.Status != Shared.Entities.Enums.EnumOrderStatus.Deleted;
    }
}
