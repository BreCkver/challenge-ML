using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Order.API.Business.Contracts;
using Order.API.Business.OrdersDetail;
using Order.API.Data;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Framework.Helpers;

namespace Order.API.IntegrationTest.OrderDetailTest
{
    [TestClass]
    public class WishListDetailFilterListTest
    {
        private WishListDetailFilterListHandler handler;
        private IUserRepository userRepository;
        private IOrderRepository orderRepository;
        private IOrderDetailRepository orderDetailRepository;

        public WishListDetailRequest GetWishListDetailRequest => new WishListDetailRequest
        {
            User = new UserDTO
            {
                Identifier = 150
            },
            WishList = new WishListDTO
            {
                Identifier = 100,
            }
        };

        [TestInitialize]
        public void Initialize()
        {
            var connectionString = Helper.GetConnection();
            userRepository = new UserData(connectionString);
            orderRepository = new OrderData(connectionString);
            orderDetailRepository = new OrderDetailData(connectionString);
            handler = new WishListDetailFilterListHandler(userRepository, orderRepository, orderDetailRepository);
        }

        [TestMethod]
        public async Task Successfull_WishListDetail_Get()
        {
            var response = await handler.Execute(GetWishListDetailRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsTrue(response.Success, errors);
        }
    }
}
