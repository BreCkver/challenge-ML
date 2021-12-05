using System.Collections.Generic;
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
    public class OrderDetailBaseValidatorTest
    {
        #region "  Vars Defined  "

        private WishListDetailHandlerMock handler;
        private Mock<IUserRepository> userRepository;
        private Mock<IOrderRepository> orderRepository;

        #endregion

        #region " Methods Initials  "

        [TestInitialize]
        public void Initialize()
        {
            userRepository = new Mock<IUserRepository>();
            orderRepository = new Mock<IOrderRepository>();
            handler = new WishListDetailHandlerMock(userRepository.Object, orderRepository.Object);
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
        public WishListDetailRequest GetWishDetailListRequestNull;

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
                BookList = new List<BookDTO>
                {
                    new BookDTO { ExternalIdentifier = "0001", Title ="Title 01", Keyword = "Titles...0"},
                    new BookDTO { ExternalIdentifier = "0002", Title ="Title 02", Keyword = "Titles...1"}
                }
            }
        };

        public static WishListDetailRequest GetWishListDetailRequestEmpty => new WishListDetailRequest
        {
            User = new UserDTO
            {
                Identifier = 0,
                Token = ""
            },
            WishList = new WishListDTO
            {
                Identifier = 88813,
                BookList = new List<BookDTO>
                {
                    new BookDTO { ExternalIdentifier = "", Title ="", Keyword = ""},
                    new BookDTO { ExternalIdentifier = "", Title ="", Keyword = ""}
                }
            }
        };

        public UserDTO GetUser = new UserDTO
        {
            Identifier = GetWishListDetailRequest.User.Identifier,
        };      

        public OrderDTO GetOrderExists = new OrderDTO
        {
            Name = "WishList Christmas",
            Status = Shared.Entities.Enums.EnumOrderStatus.Active,
            Identifier = 82247
        };

        #endregion

        #region "  Test Methods  "

        [TestMethod]
        public async Task Failure_Validations_RequestNull()
        {
            var response = await handler.IsValid(GetWishDetailListRequestNull);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestEmpty()
        {
            var response = await handler.IsValid(GetWishListDetailRequestEmpty);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestUserNoExists()
        {
            userRepository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUserNull)));

            var response = await handler.IsValid(GetWishListDetailRequest);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestUsertException()
        {
            userRepository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(GetUserNull, false)));

            var response = await handler.IsValid(GetWishListDetailRequest);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestWLException()
        {
            orderRepository.Setup(x => x.GetOrder(It.IsAny<OrderDTO>(), It.IsAny<int>()))
              .Returns(Task.FromResult(ResponseGeneric.Create(GetOrderExists, false)));

            var response = await handler.IsValid(GetWishListDetailRequest);
            Assert.IsFalse(response.Success);
        }


        [TestMethod]
        public async Task Failure_Validations_RequestWLNull()
        {
            orderRepository.Setup(x => x.GetOrder(It.IsAny<OrderDTO>(), It.IsAny<int>()))
              .Returns(Task.FromResult(ResponseGeneric.Create(GetOrderNull)));

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
        public async Task Successfull_Execute()
        {
            var response = await handler.Execute(GetWishListDetailRequest);
            Assert.IsTrue(response.Success);
        }

        #endregion
    }
}
