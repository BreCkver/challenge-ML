using System.Threading.Tasks;
using Order.API.Business.Contracts;
using Order.API.Business.Contracts.Error;
using Order.API.Business.Validations.Orders;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Constants;
using Order.API.Shared.Entities.Enums;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;
using Order.API.Shared.Framework.Helpers;

namespace Order.API.Business.Orders
{
    public class WishListCreatedHandler : OrderBaseValidator, ICommandHandler<WishListRequest, WishListResponse>
    {
        private readonly IOrderRepository orderRepository;

        public WishListCreatedHandler(IUserRepository userRepository, IOrderRepository orderRepository) : base(userRepository, orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<ResponseGeneric<WishListResponse>> Execute(WishListRequest request)
        {
            var orderValidated = await IsValid(request);
            if (orderValidated.Success)
            {
                var orderInsertResult = await orderRepository.Insert(request.WishList, request.User.Identifier);
                if (orderInsertResult.Success)
                {
                    var withlistNew = orderInsertResult.Value;
                    var respose = new WishListResponse { WishList = new WishListDTO { Identifier = withlistNew.Identifier, Name = withlistNew.Name, Status = EnumOrderStatus.Active } };
                    return ResponseGeneric.Create(respose);
                }
                return orderInsertResult.AsError<WishListResponse>();
            }
            return ResponseGeneric.CreateError<WishListResponse>(orderValidated.ErrorList);
        }

        public async Task<ResponseGeneric<bool>> IsValid(WishListRequest request)
        {
            var validationResult = await base.IsRequestValid(request);
            if (validationResult.Failure)
                return ResponseGeneric.CreateError<bool>(validationResult.ErrorList);
            return ResponseGeneric.Create(true);
        }

        protected override bool ValidatedEmptys(WishListRequest request)
       =>
            string.IsNullOrWhiteSpace(request.User.Token) ||
             request.User.Identifier == default ||
              string.IsNullOrWhiteSpace(request.WishList.Name) ||
               request.WishList.Name.Length > 50;

        protected override async Task<ResponseGeneric<bool>> ValidateOrder(WishListRequest request)
        {
            var wishListExistsResult = await orderRepository.GetOrder(request.ConverToOrderDTO(), request.User.Identifier);
            if (wishListExistsResult.Failure)
            {
                return wishListExistsResult.AsError<bool>();
            }
            if (wishListExistsResult.Value != null)
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.WISHLIST_EXISTS, ErrorMessage.WISHLIST_EXISTS, ErrorType.BUSINESS));
            }
            return ResponseGeneric.Create(true);
        }

    }
}
