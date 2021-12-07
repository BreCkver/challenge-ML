﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Order.API.Business.Contracts;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Parent;
using Order.API.Shared.Entities.Request;
using Order.API.UnitTest.Mocks;

namespace Order.API.UnitTest
{
    [TestClass]
    public class OrderBaseValidatorTest
    {

        #region "  Vars Defined  "
        private WishListHandlerMock handler;
        private Mock<IUserRepository> userRepository;
        private Mock<IOrderRepository> orderRepository;

        #endregion

        #region " Methods Initials  "

        [TestInitialize]
        public void Initialize()
        {
            userRepository = new Mock<IUserRepository>();
            orderRepository = new Mock<IOrderRepository>();
            handler = new WishListHandlerMock(userRepository.Object, orderRepository.Object);
            Setup();
        }

        public void Setup()
        {
            userRepository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                .Returns(Task.FromResult(ResponseGeneric.Create(GetUser)));

            orderRepository.Setup(x => x.GetOrder(It.IsAny<OrderDTO>(), It.IsAny<int>()))
                .Returns(Task.FromResult(ResponseGeneric.Create(GetOrderExists)));

        }

        #endregion

        #region "  Properties  "

        public UserDTO GetUserNull;
        public OrderDTO GetOrderNull;
        public WishListRequest GetWishListRequestNull;

        public UserDTO GetUser = new UserDTO
        {
            Identifier = GetWishListRequest.User.Identifier,
        };
        public static WishListRequest GetWishListRequestEmpty => new WishListRequest
        {
            User = new UserDTO
            {
                Identifier = 0,
                Token = ""
            },
            WishList = new WishListDTO
            {
                Name = ""
            }
        };
        public static WishListRequest GetWishListRequest => new WishListRequest
        {
            User = new UserDTO
            {
                Identifier = 12313,
                Token = "TokenEmpty"
            },
            WishList = new WishListDTO
            {
                Name = "My first WishList"
            }
        };

        public OrderDTO GetOrderExists = new OrderDTO
        {
            Name = "Exists",
            Status = Shared.Entities.Enums.EnumOrderStatus.Active,
            Identifier = 1234
        };

        #endregion

        #region "  Test Methods  "

        [TestMethod]
        public async Task Failure_Validations_RequestNull()
        {
            var response = await handler.IsValid(GetWishListRequestNull);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestEmpty()
        {
            var response = await handler.IsValid(GetWishListRequestEmpty);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestUserNoExists()
        {
            userRepository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUserNull)));

            var response = await handler.IsValid(GetWishListRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestUsertException()
        {
            userRepository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(GetUserNull, false)));

            var response = await handler.IsValid(GetWishListRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestWLException()
        {
            orderRepository.Setup(x => x.GetOrder(It.IsAny<OrderDTO>(), It.IsAny<int>()))
              .Returns(Task.FromResult(ResponseGeneric.Create(GetOrderExists, false)));

            var response = await handler.IsValid(GetWishListRequest);

            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }


        [TestMethod]
        public async Task Failure_Validations_RequestWLNull()
        {
            orderRepository.Setup(x => x.GetOrder(It.IsAny<OrderDTO>(), It.IsAny<int>()))
              .Returns(Task.FromResult(ResponseGeneric.Create(GetOrderNull)));

            var response = await handler.IsValid(GetWishListRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }

        [TestMethod]
        public async Task Successfull_Validations()
        {
            var response = await handler.IsValid(GetWishListRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsTrue(response.Success, errors);
        }

        [TestMethod]
        public async Task Successfull_Execute()
        {
            var response = await handler.Execute(GetWishListRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsTrue(response.Success, errors);
        }

        #endregion
    }
}
