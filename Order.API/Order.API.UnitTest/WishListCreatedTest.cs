using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Order.API.Business.Contracts;
using Order.API.Business.Orders;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Parent;
using Order.API.Shared.Entities.Request;

namespace Order.API.UnitTest
{
    [TestClass]
    public class WishListCreatedTest
    {
        private WishListCreatedHandler handler;
        private Mock<IUserRepository> userRepository;
        private Mock<IOrderRepository> orderRepository;

        [TestInitialize]
        public void Initialize()
        {
            userRepository = new Mock<IUserRepository>();
            orderRepository = new Mock<IOrderRepository>();
            handler = new WishListCreatedHandler(userRepository.Object, orderRepository.Object);
            Setup();
        }
        public void Setup()
        {
            userRepository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                .Returns(Task.FromResult(ResponseGeneric.Create(GetUser)));

            orderRepository.Setup(x => x.GetOrder(It.IsAny<OrderDTO>(), It.IsAny<int>()))
                .Returns(Task.FromResult(ResponseGeneric.Create(orderNull)));
        }

        [TestMethod]
        public async Task Failure_Validations_RequestNull()
        {
            var response = await handler.IsValid(GetWishListRequestNull);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestEmpty()
        {
            var response = await handler.IsValid(GetWishListRequestEmpty);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestUserNoExists()
        {
            userRepository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUserNull)));

            var response = await handler.IsValid(GetWishListRequest);
            Assert.IsFalse(response.Success);
        }


        [TestMethod]
        public async Task Failure_Validations_RequestUsertException()
        {
            userRepository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(GetUserNull, false)));

            var response = await handler.IsValid(GetWishListRequest);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestWLExists()
        {
            orderRepository.Setup(x => x.GetOrder(It.IsAny<OrderDTO>(), It.IsAny<int>()))
               .Returns(Task.FromResult(ResponseGeneric.Create(orderExists)));

            var response = await handler.IsValid(GetWishListRequest);
            Assert.IsFalse(response.Success);
        }


        [TestMethod]
        public async Task Failure_Validations_RequestWLException()
        {
            orderRepository.Setup(x => x.GetOrder(It.IsAny<OrderDTO>(), It.IsAny<int>()))
              .Returns(Task.FromResult(ResponseGeneric.Create(orderExists, false)));

            var response = await handler.IsValid(GetWishListRequest);
            Assert.IsFalse(response.Success);
        }


        [TestMethod]
        public async Task Successfull_Validations()
        {
            var response = await handler.IsValid(GetWishListRequest);
            Assert.IsTrue(response.Success);
        }


        public WishListCreatedRequest GetWishListRequestNull;

        public static WishListCreatedRequest GetWishListRequestEmpty => new WishListCreatedRequest
        {
            User = new UserDTO
            {
                Identifier = 0,
                Token = ""
            },
            Name = ""
        };

        public static WishListCreatedRequest GetWishListRequest => new WishListCreatedRequest
        {
            User = new UserDTO
            {
                Identifier = 8721,
                Token = "Token1231"
            },
            Name = "Mi primer WishList"
        };

        public UserDTO GetUser = new UserDTO
        {
            Identifier = GetWishListRequest.User.Identifier,
        };
        public UserDTO GetUserNull;
        public OrderDTO orderNull;
        public OrderDTO orderExists = new OrderDTO
        {
            Name = "Exists",
            Status = Shared.Entities.Enums.EnumOrderStatus.Active,
            Identifier = 1234
        };
    }
}
