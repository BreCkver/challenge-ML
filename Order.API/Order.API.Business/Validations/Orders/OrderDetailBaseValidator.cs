using Order.API.Business.Contracts;
using Order.API.Business.Contracts.Error;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Constants;
using Order.API.Shared.Entities.Parent;
using Order.API.Shared.Entities.Request;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.Business.Validations.Orders
{
    public abstract class OrderDetailBaseValidator : IValidator<WishListDetailRequest>
    {
        private readonly IUserRepository userRepository;
        private readonly IOrderRepository orderRepository;
     
        public OrderDetailBaseValidator(IUserRepository userRepository, IOrderRepository orderRepository)
        {
            this.userRepository = userRepository;
            this.orderRepository = orderRepository;
          
        }

        public async Task<ResponseGeneric<bool>> IsRequestValid(WishListDetailRequest request)
        {
            if (ValidateNulls(request))
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
            if (validateOrderResult.Failure)
            {
                return validateOrderResult.AsError<bool>();
            }

            var productExistsResult = await ValidateProductExists(request);
            if (productExistsResult.Failure)
            {
                return ResponseGeneric.CreateError<bool>(productExistsResult.ErrorList);
            }

            return ResponseGeneric.Create(true);
        }

        protected virtual async Task<ResponseGeneric<bool>> ValidateOrder(WishListDetailRequest request)
        {
            var wishListExistsResult = await orderRepository.GetOrder(request.WishList, request.User.Identifier);
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

        protected abstract bool ValidatedEmptys(WishListDetailRequest request);

        protected virtual bool ValidateNulls(WishListDetailRequest request) 
            => request == null || request.User == null || request.WishList == null || request.WishList.BookList == null;

        protected abstract Task<ResponseGeneric<bool>> ValidateProductExists(WishListDetailRequest request);

        protected virtual IEnumerable<ProductDTO> GetMatchElements(IEnumerable<BookDTO> bookListDB, IEnumerable<BookDTO> bookListReq) =>
         bookListDB.Join(bookListReq,
                 externalIdBD => externalIdBD.ExternalIdentifier,
                 externalIdRequest => externalIdRequest.ExternalIdentifier,
                 (elementDB, elementRequest) => new ProductDTO
                 {
                     Identifier = elementDB.Identifier,
                     Description = elementDB.Description,
                     ExternalIdentifier = elementDB.ExternalIdentifier
                 });
    }
}
