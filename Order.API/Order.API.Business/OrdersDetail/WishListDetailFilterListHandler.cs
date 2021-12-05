using System.Linq;
using System.Threading.Tasks;
using Order.API.Business.Contracts;
using Order.API.Business.Validations.Orders;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;

namespace Order.API.Business.OrdersDetail
{
    public class WishListDetailFilterListHandler : OrderDetailBaseValidator, ICommandHandler<WishListDetailRequest, WishListDetailResponse>
    {
        private readonly IOrderDetailRepository orderDetailRepository;

        public WishListDetailFilterListHandler(IUserRepository userRepository, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository)
            : base(userRepository, orderRepository)
        {
            this.orderDetailRepository = orderDetailRepository;
        }

        public async Task<ResponseGeneric<WishListDetailResponse>> Execute(WishListDetailRequest request)
        {
            var orderValidated = await IsValid(request);
            if (orderValidated.Success)
            {
                var orderListResult = await orderDetailRepository.GetAllByOrder(request.WishList);
                if (orderListResult.Success)
                {
                    var respose = new WishListDetailResponse { WishList = new WishListDTO { BookList = orderListResult.Value.ToList() } };
                    return ResponseGeneric.Create(respose);
                }
                return orderListResult.AsError<WishListDetailResponse>();
            }
            return orderValidated.AsError<WishListDetailResponse>();
        }

        public async Task<ResponseGeneric<bool>> IsValid(WishListDetailRequest request)
        {
            var validationResult = await base.IsRequestValid(request);
            if (validationResult.Failure)
                return validationResult.AsError<bool>();
            return ResponseGeneric.Create(true);
        }

        protected override bool ValidatedEmptys(WishListDetailRequest request)
        =>  request.User.Identifier == default || request.WishList.Identifier == default;

        protected override Task<ResponseGeneric<bool>> ValidateProductExists(WishListDetailRequest request)
        => Task.FromResult(ResponseGeneric.Create(true));
    }
}
