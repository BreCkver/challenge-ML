using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Order.API.Business.Contracts;
using Order.API.Business.Contracts.Error;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Constants;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;
using Order.API.Shared.Framework.Helpers;

namespace Order.API.Business.Orders
{
    public class WishListFilterListHandler : ICommandHandler<WishListFilterRequest, WishListFilterResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IOrderRepository orderRepository;

        public WishListFilterListHandler(IUserRepository userRepository, IOrderRepository orderRepository)
        {
            this.userRepository = userRepository;
            this.orderRepository = orderRepository;
        }

        public async Task<ResponseGeneric<WishListFilterResponse>> Execute(WishListFilterRequest request)
        {
            var filterValidated = await IsValid(request);
            if (filterValidated.Success)
            {
                var wishListsResult = await orderRepository.GetAllByUser(request.User.Identifier);
                if (wishListsResult.Failure)
                {
                    return wishListsResult.AsError<WishListFilterResponse>();
                }
                if (wishListsResult.Value == null)
                {
                    return ResponseGeneric.CreateError<WishListFilterResponse>(new Error(ErrorCode.WISHLIST_EXISTS, ErrorMessage.WISHLIST_EXISTS, ErrorType.BUSINESS));
                }
                var wishList = wishListsResult.Value == null || !wishListsResult.Value.Any() ? new List<WishListDTO>() : wishListsResult.Value.ConvertToWishLists();
                var response = new WishListFilterResponse { WishLists = wishList };
                return ResponseGeneric.Create(response);
            }
            return ResponseGeneric.CreateError<WishListFilterResponse>(filterValidated.ErrorList);
        }

        public async Task<ResponseGeneric<bool>> IsValid(WishListFilterRequest request)
        {
            if (request == null || request.User == null)
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
            return ResponseGeneric.Create(true);
        }

        private bool ValidateRequest(WishListFilterRequest request) =>
         string.IsNullOrWhiteSpace(request.User.Token) ||
           request.User.Identifier == default;
    }
}
