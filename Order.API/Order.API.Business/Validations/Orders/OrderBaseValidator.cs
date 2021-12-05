using Order.API.Business.Contracts;
using Order.API.Business.Contracts.Error;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Constants;
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

        public async Task<ResponseGeneric> IsRequestValid(WishListRequest request)
        {
            if (request == null || request.User == null || request.WishList == null)
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.REQUEST_NULL, ErrorMessage.REQUEST_NULL, ErrorType.BUSINESS));
            }
            if (ValidatedEmptys(request))
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
            var validateOrderResult = await ValidateOrder(request);
            if(validateOrderResult.Failure)
            {
                return validateOrderResult.AsError<bool>();
            }

            return ResponseGeneric.Create(true);
        }

        protected virtual async Task<ResponseGeneric<bool>> ValidateOrder(WishListRequest request)
        {
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

        protected abstract bool ValidatedEmptys(WishListRequest request);
    }
}
