using System.Linq;
using System.Threading.Tasks;
using Order.API.Business.Contracts;
using Order.API.Business.Contracts.Error;
using Order.API.Business.Validations.Orders;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Constants;
using Order.API.Shared.Entities.Enums;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;

namespace Order.API.Business.OrdersDetail
{
    public class WishListDetailUpdatedHandler : OrderDetailBaseValidator, ICommandHandler<WishListDetailRequest, WishListDetailResponse>
    {
        private readonly IOrderDetailRepository orderDetailRepository;
        public WishListDetailUpdatedHandler(IUserRepository userRepository, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository) 
            : base(userRepository, orderRepository)
        {
            this.orderDetailRepository = orderDetailRepository;
        }

        public async Task<ResponseGeneric<WishListDetailResponse>> Execute(WishListDetailRequest request)
        {
            var orderValidated = await IsValid(request);
            if (orderValidated.Success)
            {
                var orderUpdatedResult = await orderDetailRepository.Update(request.WishList, request.WishList.BookList);
                if (orderUpdatedResult.Success)
                {
                    var respose = new WishListDetailResponse { WishList = request.WishList };
                    return ResponseGeneric.Create(respose);
                }
                return orderUpdatedResult.AsError<WishListDetailResponse>();
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
        =>
             request.User.Identifier == default ||
              request.WishList.BookList.Any(b => b.Identifier == default) ||
                request.WishList.Status != EnumOrderStatus.None;

        protected override async Task<ResponseGeneric<bool>> ValidateProductExists(WishListDetailRequest request)
        {
            var productExistsResult = await orderDetailRepository.GetAllByOrder(request.WishList);
            if (productExistsResult.Failure)
            {
                return productExistsResult.AsError<bool>();
            }

            if (productExistsResult.Value == null || !productExistsResult.Value.Any())
            {
                return ResponseGeneric.CreateError<bool>(new Error
                  (
                      ErrorCode.PRODUCT_NOEXISTS,
                      ErrorMessage.PRODUCT_NOEXISTS,
                      ErrorType.BUSINESS
                  ));
            }

            var existsDuplicate = GetMatchElements(productExistsResult.Value, request.WishList.BookList);
            if (existsDuplicate.Any() && existsDuplicate.Count() != request.WishList.BookList.Count())
            {

                var booksNoExists = request.WishList.BookList.Where(e => !existsDuplicate.Any(du => du.ExternalIdentifier == e.ExternalIdentifier));
                return ResponseGeneric.CreateError<bool>(new Error
                    (
                        ErrorCode.PRODUCT_NOEXISTS,
                        string.Format(ErrorMessage.PRODUCT_NOEXISTS, string.Join(",", booksNoExists.Select(d => d.ExternalIdentifier))),
                        ErrorType.BUSINESS)
                    );
            }
            return ResponseGeneric.Create(true);
        }
    }
}
