using Order.API.Business.Contracts;
using Order.API.Business.OrdersDetail;
using Order.API.Data;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Enums;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;
using Order.API.Shared.Framework.Helpers;
using System.Linq;

namespace Order.API.Host.Factory
{
    /// <summary>
    /// 
    /// </summary>
    public class WishListDetailFactory
    {
        private readonly string connectionString;
        /// <summary>
        /// 
        /// </summary>
        public WishListDetailFactory()
        {
            connectionString = Helper.GetConnection();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseGeneric<ICommandHandler<WishListDetailRequest, WishListDetailResponse>> Create(WishListDetailRequest request)
        {
            var data = new UserData(connectionString);
            var orderData = new OrderData(connectionString);
            var orderDetailData = new OrderDetailData(connectionString);

            if(request.WishList.BookList != null && request.WishList.BookList.Any(b => b.Status != EnumProductStatus.Deleted))
            {
                var handler = new WishListDetailAddHandler(data, orderData, orderDetailData);
                return ResponseGeneric.Create((ICommandHandler<WishListDetailRequest, WishListDetailResponse>)handler);
            }
            else if(request.WishList.BookList != null && request.WishList.BookList.Any(b => b.Status == EnumProductStatus.Deleted))
            {
                var handler = new WishListDetailUpdatedHandler(data, orderData, orderDetailData);
                return ResponseGeneric.Create((ICommandHandler<WishListDetailRequest, WishListDetailResponse>)handler);
            }
            else
            {
                var handler = new WishListDetailFilterListHandler(data, orderData, orderDetailData);
                return ResponseGeneric.Create((ICommandHandler<WishListDetailRequest, WishListDetailResponse>)handler);
            }
        }
    }
}