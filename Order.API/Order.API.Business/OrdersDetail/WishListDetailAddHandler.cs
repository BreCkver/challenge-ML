using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Order.API.Business.Contracts;
using Order.API.Business.Contracts.Error;
using Order.API.Business.Validations.Orders;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Constants;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;


namespace Order.API.Business.OrdersDetail
{
    public class WishListDetailAddHandler : OrderDetailBaseValidator, ICommandHandler<WishListDetailRequest, WishListDetailResponse>
    {
        private readonly IOrderDetailRepository orderDetailRepository;
        public WishListDetailAddHandler(IUserRepository userRepository, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository) :
            base(userRepository, orderRepository)
        {
            this.orderDetailRepository = orderDetailRepository;
        }

        public async Task<ResponseGeneric<WishListDetailResponse>> Execute(WishListDetailRequest request)
        {
            var orderValidated = await IsValid(request);
            if (orderValidated.Success)
            {
                var orderInsertResult = await orderDetailRepository.Add(request.WishList, request.WishList.BookList);
                if (orderInsertResult.Success)
                {
                    var respose = new WishListDetailResponse { WishList = request.WishList };
                    return ResponseGeneric.Create(respose);
                }
                return orderInsertResult.AsError<WishListDetailResponse>();
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
             request.WishList.Identifier == default ||
                ValidateBooks(request.WishList.BookList);

        private bool ValidateBooks(IEnumerable<BookDTO> bookList)
        =>
            bookList.ToList().Any(book => string.IsNullOrWhiteSpace(book.Keyword) || string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.ExternalIdentifier));

        protected override async Task<ResponseGeneric<bool>> ValidateProductExists(WishListDetailRequest request)
        {
            var productExistsResult = await orderDetailRepository.GetAllByOrder(request.WishList);
            if (productExistsResult.Failure)
            {
                return productExistsResult.AsError<bool>();
            }

            if(productExistsResult.Value == null || !productExistsResult.Value.Any())
            {
                return ResponseGeneric.Create(true);
            }

            var existsDuplicate = GetMatchElements(productExistsResult.Value, request.WishList.BookList);
            if (existsDuplicate.Any())
            {
                return ResponseGeneric.CreateError<bool>(new Error
                    (
                        ErrorCode.PRODUCT_EXISTS,
                        string.Format(ErrorMessage.PRODUCT_EXISTS, string.Join(",", existsDuplicate.Select(d => d.ExternalIdentifier))),
                        ErrorType.BUSINESS)
                    );
            }
            return ResponseGeneric.Create(true);
        }     
    }
}
