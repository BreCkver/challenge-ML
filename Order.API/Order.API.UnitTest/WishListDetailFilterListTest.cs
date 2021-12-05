﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Order.API.Business.Contracts;
using Order.API.Business.OrdersDetail;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Parent;
using Order.API.Shared.Entities.Request;

namespace Order.API.UnitTest
{
    [TestClass]
    public class WishListDetailFilterListTest
    {
        #region "  Vars Defined  "


        private WishListDetailFilterListHandler handler;
        private Mock<IUserRepository> userRepository;
        private Mock<IOrderRepository> orderRepository;
        private Mock<IOrderDetailRepository> orderDetailRepository;

        #endregion

        #region " Methods Initials  "

        [TestInitialize]
        public void Initialize()
        {
            userRepository = new Mock<IUserRepository>();
            orderRepository = new Mock<IOrderRepository>();
            orderDetailRepository = new Mock<IOrderDetailRepository>();
            handler = new WishListDetailFilterListHandler(userRepository.Object, orderRepository.Object, orderDetailRepository.Object);
            Setup();
        }
        public void Setup()
        {
            userRepository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                .Returns(Task.FromResult(ResponseGeneric.Create(GetUser)));

            orderRepository.Setup(x => x.GetOrder(It.IsAny<OrderDTO>(), It.IsAny<int>()))
             .Returns(Task.FromResult(ResponseGeneric.Create(GetOrderExists)));

            orderDetailRepository.Setup(x => x.GetAllByOrder(It.IsAny<OrderDTO>()))
                .Returns(Task.FromResult(ResponseGeneric.Create(GetBookList)));
        }

        #endregion

        #region "  Properties  "

        public static WishListDetailRequest GetWishListDetailRequest => new WishListDetailRequest
        {
            User = new UserDTO
            {
                Identifier = 12313,
                Token = "TokenEmpty"
            },
            WishList = new WishListDTO
            {
                Identifier = 88813,
                BookList = GetBookList.ToList(),
            }
        };

        public static IEnumerable<BookDTO> GetBookList = new List<BookDTO>
        {
            new BookDTO { ExternalIdentifier = "0001", Title ="Title 01", Keyword = "Titles...0", Identifier = 1001},
            new BookDTO { ExternalIdentifier = "0002", Title ="Title 02", Keyword = "Titles...1", Identifier = 1002}
        };       

        public UserDTO GetUser = new UserDTO
        {
            Identifier = GetWishListDetailRequest.User.Identifier,
        };

        public OrderDTO GetOrderExists = new OrderDTO
        {
            Name = "WishList New Year",
            Status = Shared.Entities.Enums.EnumOrderStatus.Active,
            Identifier = 55444
        };


        #endregion

        #region "  Test Methods  "


        [TestMethod]
        public async Task Failure_Validations_ProductsNoExists()
        {
            userRepository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(GetUser, false)));

            var response = await handler.IsValid(GetWishListDetailRequest);
            Assert.IsFalse(response.Success);
        }       
     

        [TestMethod]
        public async Task Successfull_Validations()
        {
            var response = await handler.IsValid(GetWishListDetailRequest);
            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        public async Task Successfull_Get()
        {
            var response = await handler.Execute(GetWishListDetailRequest);
            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        public async Task Failure_Get_WithException()
        {
            orderDetailRepository.Setup(x => x.GetAllByOrder(It.IsAny<OrderDTO>()))
                 .Returns(Task.FromResult(ResponseGeneric.Create(GetBookList, false)));

            var response = await handler.Execute(GetWishListDetailRequest);
            Assert.IsFalse(response.Success);
        }

        #endregion
    }
}
