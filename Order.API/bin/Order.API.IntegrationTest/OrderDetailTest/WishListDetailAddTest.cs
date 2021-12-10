using System;
using System.Collections.Generic;
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
    public class WishListDetailAddTest
    {
        private WishListDetailAddHandler handler;
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
                BookList = GetBookList.ToList(),
            }
        };

        public IEnumerable<BookDTO> GetBookList = new List<BookDTO>
        {
            new BookDTO { ExternalIdentifier = GetExternalIdentifier(), Title ="Title 01", Keyword = "Titles 0", Authors = new List<string>{"Author 01"}, Publisher="Publisher 01"}
        };


        [TestInitialize]
        public void Initialize()
        {
            var connectionString = Helper.GetConnection();
            userRepository = new UserData(connectionString);
            orderRepository = new OrderData(connectionString);
            orderDetailRepository = new OrderDetailData(connectionString);
            handler = new WishListDetailAddHandler(userRepository, orderRepository, orderDetailRepository);
        }

        [TestMethod]
        public async Task Successfull_WishListDetail_Add()
        {
            var response = await handler.Execute(GetWishListDetailRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsTrue(response.Success, errors);
        }

        private static string GetExternalIdentifier()
        {
            Random random = new Random();
            int ranPrefix = random.Next(0, 10000);
            int ranSufix = random.Next(10000, 20000);
            return $"External{ranPrefix}-{ranSufix}";
        }

    }
}
