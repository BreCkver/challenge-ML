using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Order.API.Business.Contracts;
using Order.API.Business.Validations.Orders;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Enums;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;
using Order.API.Shared.Framework.Helpers;

namespace Order.API.Business.Orders
{
    public class WishListFilterListHandler : OrderBaseValidator, ICommandHandler<WishListRequest, WishListFilterResponse>
    {
        private readonly IOrderRepository orderRepository;

        public WishListFilterListHandler(IUserRepository userRepository, IOrderRepository orderRepository) : base(userRepository, orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<ResponseGeneric<WishListFilterResponse>> Execute(WishListRequest request)
        {
            var filterValidated = await IsValid(request);
            if (filterValidated.Success)
            {
                var wishListsResult = await orderRepository.GetAllByUser(request.User.Identifier.Value);
                if (wishListsResult.Failure)
                {
                    return wishListsResult.AsError<WishListFilterResponse>();
                }
                var wishList = 
                        wishListsResult.Value == null || !wishListsResult.Value.Any() 
                        ? new List<WishListDTO>() 
                        : wishListsResult.Value.Where(wl => wl.Status == EnumOrderStatus.Active).ConvertToWishLists();
                var response = new WishListFilterResponse { WishLists = wishList };
                return ResponseGeneric.Create(response);
            }
            return ResponseGeneric.CreateError<WishListFilterResponse>(filterValidated.ErrorList);
        }

        public async Task<ResponseGeneric<bool>> IsValid(WishListRequest request)
        {
            var validationResult = await IsRequestValid(request);
            if (validationResult.Failure)
                return ResponseGeneric.CreateError<bool>(validationResult.ErrorList);
            return ResponseGeneric.Create(true);
        }

        protected override bool ValidateRequest(WishListRequest request) =>
           request.User.Identifier == default;

        protected override Task<ResponseGeneric<bool>> ValidateOrder(WishListRequest request)
        {
            return Task.FromResult(ResponseGeneric.Create(true));
        }
    }
}
