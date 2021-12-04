using System.Threading.Tasks;
using Order.API.Business.Contracts;
using Order.API.Business.Contracts.Error;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Constants;
using Order.API.Shared.Entities.Enums;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;
using Order.API.Shared.Framework.Helpers;

namespace Order.API.Business.Orders
{
    //WishListResponse
    //WishListRequest

    public class WishListUpdatedHandler : ICommandHandler<WishListRequest, WishListResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IOrderRepository orderRepository;

        public WishListUpdatedHandler(IUserRepository userRepository, IOrderRepository orderRepository)
        {
            this.userRepository = userRepository;
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
            if (request == null || request.User == null || request.WishList == null)
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.REQUEST_NULL, ErrorMessage.REQUEST_NULL, ErrorType.BUSINESS));
            }
            if (ValidateRequest(request))
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.REQUEST_EMPTY, ErrorMessage.REQUEST_EMPTY, ErrorType.BUSINESS));
            }

            var UserExistsResult = await userRepository.GetByUser(request.User);
            if (UserExistsResult.Failure)
            {
                return UserExistsResult.AsError<bool>();
            }
            if (UserExistsResult.Value == null)
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.USER_NOEXISTS, ErrorMessage.USER_NOEXISTS, ErrorType.BUSINESS));
            }

            var wishListExistsResult = await orderRepository.GetOrder(request.ConverToOrderDTO(), request.User.Identifier);
            if (wishListExistsResult.Failure)
            {
                return wishListExistsResult.AsError<bool>();
            }
            if (wishListExistsResult.Value == null)
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.WISHLIST_NOEXISTS, ErrorMessage.WISHLIST_NOEXISTS, ErrorType.BUSINESS));
            }

            return ResponseGeneric.Create(true);
        }

        private bool ValidateRequest(WishListRequest request) =>
         string.IsNullOrWhiteSpace(request.User.Token) ||
           request.User.Identifier == default ||
            request.WishList.Identifier == default;
    }
}
