using Order.API.Business.Contracts;
using Order.API.Business.Contracts.Error;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Constants;
using Order.API.Shared.Entities.Enums;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Framework.Helpers;
using System.Threading.Tasks;

namespace Order.API.Business.Validations.Orders
{
    public abstract class OrderBaseValidator : IValidator<WishListRequest>
    {
        private readonly IUserRepository userRepository;
        private readonly IOrderRepository orderRepository;
        public OrderBaseValidator(IUserRepository userRepository, IOrderRepository orderRepository)
        {
            this.userRepository = userRepository;
            this.orderRepository = orderRepository;
        }

        public async Task<ResponseGeneric<bool>> IsRequestValid(WishListRequest request)
        {
            if (ValidateNulls(request))
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.REQUEST_NULL, ErrorMessage.REQUEST_NULL, ErrorType.BUSINESS));
            }
            if (ValidateRequest(request))
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.REQUEST_WRONG, ErrorMessage.REQUEST_WRONG, ErrorType.BUSINESS));
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
            var validateOrderResult = await ValidateOrder(request);
            if(validateOrderResult.Failure)
            {
                return validateOrderResult.AsError<bool>();
            }

            return ResponseGeneric.Create(true);
        }

        protected virtual async Task<ResponseGeneric<bool>> ValidateOrder(WishListRequest request)
        {
            var wishListExistsResult = await orderRepository.GetOrder(request.WishList, request.User.Identifier.Value);
            if (wishListExistsResult.Failure)
            {
                return wishListExistsResult.AsError<bool>();
            }
            if (wishListExistsResult.Value == null || wishListExistsResult.Value.Status != (int)EnumOrderStatus.Active)
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.WISHLIST_NOEXISTS, ErrorMessage.WISHLIST_NOEXISTS, ErrorType.BUSINESS));
            }
            return ResponseGeneric.Create(true);
        }

        protected virtual bool ValidateNulls(WishListRequest request)
           => request == null || request.User == null || request.WishList == null;
        protected abstract bool ValidateRequest(WishListRequest request);
    }
}
