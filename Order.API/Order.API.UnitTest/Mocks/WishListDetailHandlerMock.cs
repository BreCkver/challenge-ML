using Order.API.Business.Contracts;
using Order.API.Business.Validations.Orders;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;
using System.Threading.Tasks;

namespace Order.API.UnitTest.Mocks
{
    public class WishListDetailHandlerMock : OrderDetailBaseValidator, ICommandHandler<WishListDetailRequest, WishListDetailResponse>
    {
        public WishListDetailHandlerMock(IUserRepository userRepository, IOrderRepository orderRepository) : base(userRepository, orderRepository)
        {
        }

        public Task<ResponseGeneric<WishListDetailResponse>> Execute(WishListDetailRequest request)
        {
            return Task.FromResult(ResponseGeneric.Create(new WishListDetailResponse {  WishList = new WishListDTO() } ));
        }

        public async Task<ResponseGeneric<bool>> IsValid(WishListDetailRequest request)
        {
            var validationResult = await base.IsRequestValid(request);
            if (validationResult.Failure)
                return ResponseGeneric.CreateError<bool>(validationResult.ErrorList);
            return ResponseGeneric.Create(true);
        }

        protected override bool ValidatedEmptys(WishListDetailRequest request)
        =>
          request.User.Identifier == default ||
           request.WishList.Identifier == default;

        protected override Task<ResponseGeneric<bool>> ValidateProductExists(WishListDetailRequest request)
        => Task.FromResult(ResponseGeneric.Create(true));
    }
}
