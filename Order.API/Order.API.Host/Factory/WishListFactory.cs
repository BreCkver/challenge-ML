using Order.API.Business.Contracts;
using Order.API.Business.Orders;
using Order.API.Data;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Enums;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;
using Order.API.Shared.Framework.Helpers;

namespace Order.API.Host.Factory
{
    public class WishListFactory
    {
        private readonly string connectionString;
        private readonly UserData userData;
        private readonly OrderData orderData;
        public WishListFactory()
        {
            connectionString = Helper.GetConnection();
            userData = new UserData(connectionString);
            orderData = new OrderData(connectionString);

        }
        public ResponseGeneric<ICommandHandler<WishListRequest, WishListResponse>> Create(WishListRequest request)
        {
            if (request != null && request.WishList != null && request.WishList.Status == (int)EnumOrderStatus.New)
            {
                var handler = new WishListCreatedHandler(userData, orderData);
                return ResponseGeneric.Create((ICommandHandler<WishListRequest, WishListResponse>)handler);
            }
            else
            {
                var handler = new WishListUpdatedHandler(userData, orderData);
                return ResponseGeneric.Create((ICommandHandler<WishListRequest, WishListResponse>)handler);
            }
        }

        public ResponseGeneric<ICommandHandler<WishListRequest, WishListFilterResponse>> CreateFilter()
        {
            var handler = new WishListFilterListHandler(userData, orderData);
            return ResponseGeneric.Create((ICommandHandler<WishListRequest, WishListFilterResponse>)handler);
        }
    }
}