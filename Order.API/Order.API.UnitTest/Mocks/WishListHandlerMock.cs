using System.Threading.Tasks;
using Order.API.Business.Contracts;
using Order.API.Business.Validations.Orders;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;

namespace Order.API.UnitTest.Mocks
{
    public class WishListHandlerMock : OrderBaseValidator, ICommandHandler<WishListRequest, WishListResponse>
    {        
        public WishListHandlerMock(IUserRepository userRepository, IOrderRepository orderRepository) : base(userRepository, orderRepository)
        {
           
        }

        public Task<ResponseGeneric<WishListResponse>> Execute(WishListRequest request)
        {
            return Task.FromResult(ResponseGeneric.Create(new WishListResponse { WishList = new WishListDTO { } }));
        }

        public async Task<ResponseGeneric<bool>> IsValid(WishListRequest request)
        {
            var validationResult = await base.IsRequestValid(request);
            if (validationResult.Failure)
                return ResponseGeneric.CreateError<bool>(validationResult.ErrorList);
            return ResponseGeneric.Create(true);
        }

        protected override bool ValidateRequest(WishListRequest request) => string.IsNullOrEmpty(request.User.Token) || string.IsNullOrWhiteSpace(request.WishList.Name);
    }
}
