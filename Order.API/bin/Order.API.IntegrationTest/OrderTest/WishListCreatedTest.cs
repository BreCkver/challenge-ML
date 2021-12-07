using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Order.API.Business.Contracts;
using Order.API.Business.Orders;
using Order.API.Data;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Framework.Helpers;

namespace Order.API.IntegrationTest.OrderTest
{
    [TestClass]
    public class WishListCreatedTest
    {
        private WishListCreatedHandler handler;
        private IUserRepository userRepository;
        private IOrderRepository orderRepository;

        public WishListRequest GetWishListRequest => new WishListRequest
        {
            User = new UserDTO
            {
                Identifier = 150,
            },
            WishList = new WishListDTO
            {
                Name = GetWishListName(),
                Status = Shared.Entities.Enums.EnumOrderStatus.New
            }
        };

        [TestInitialize]
        public void Initialize()
        {
            var connectionString = Helper.GetConnection();
            userRepository = new UserData(connectionString);
            orderRepository = new OrderData(connectionString);
            handler = new WishListCreatedHandler(userRepository, orderRepository);
        }

        [TestMethod]
        public async Task Successfull_WishList_Created()
        {
            var response = await handler.Execute(GetWishListRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsTrue(response.Success, errors);
        }

        private string GetWishListName()
        {
            Random random = new Random();
            int ranPrefix = random.Next(0, 10000);
            int ranSufix = random.Next(10000, 20000);
            return $"WishListName{ranPrefix}-{ranSufix}";
        }
    }
}
