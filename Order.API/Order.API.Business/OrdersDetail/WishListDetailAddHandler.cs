using System;
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
using Order.API.Shared.Framework.Helpers;

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
            try
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
            catch (Exception ex)
            {
                return ResponseGeneric.CreateError<WishListDetailResponse>(new Error(ErrorCode.INTERNAL_ERROR, ex));
            }
        }

        public async Task<ResponseGeneric<bool>> IsValid(WishListDetailRequest request)
        {
            var validationResult = await base.IsRequestValid(request);
            if (validationResult.Failure)
            {
                return validationResult.AsError<bool>();
            }            
            if(!ValidateBooksSpecialCharacters(request.WishList.BookList))
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.REQUEST_NOALPHANUMERIC, ErrorMessage.REQUEST_NOALPHANUMERIC, ErrorType.BUSINESS));
            }
            return ResponseGeneric.Create(true);
        }

        protected override bool ValidateRequest(WishListDetailRequest request)
        =>
            request.User.Identifier == default ||
              request.WishList.Identifier == default ||
                ValidateBooks(request.WishList.BookList);

        private bool ValidateBooks(IEnumerable<BookDTO> bookList)
        =>
            bookList.ToList().Any(book =>
                        string.IsNullOrWhiteSpace(book.Title) || 
                            string.IsNullOrWhiteSpace(book.ExternalIdentifier) ||
                               book.Publisher == null || 
                                (book.Authors == null || book.Authors.Count <= 0));

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


        private bool ValidateBooksSpecialCharacters(IEnumerable<BookDTO> bookList)
        {
            foreach(var book in bookList)
            {
                if (book.Title.ToLower().ValidateCharactersSpecial() &&
                     book.Authors.All(a => a.ToLower().ValidateCharactersSpecial()) &&
                     book.Publisher.ToLower().ValidateCharactersSpecial())
                {
                    continue;
                }
                else
                    return false;
            }
            return true;
        }
    }
}
