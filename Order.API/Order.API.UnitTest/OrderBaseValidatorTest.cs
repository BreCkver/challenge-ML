using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Order.API.Business.Contracts;
using Order.API.Business.Orders;
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

        public IEnumerable<OrderDTO> GetOrderList = new List<OrderDTO>
        {
            new OrderDTO { Identifier = 1, Name = "Est", Status = Shared.Entities.Enums.EnumOrderStatus.Active},
            new OrderDTO { Identifier = 2, Name = "Act", Status = Shared.Entities.Enums.EnumOrderStatus.Active}
        };

        #endregion

        #region "  Test Methods  "

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
        public async Task Failure_Validations_RequestWLException()
        {
            orderRepository.Setup(x => x.GetOrder(It.IsAny<OrderDTO>(), It.IsAny<int>()))
              .Returns(Task.FromResult(ResponseGeneric.Create(GetOrderExists, false)));

            var response = await handler.IsValid(GetWishListRequest);
            Assert.IsFalse(response.Success);
        }


        [TestMethod]
        public async Task Failure_Validations_RequestWLNull()
        {
            orderRepository.Setup(x => x.GetOrder(It.IsAny<OrderDTO>(), It.IsAny<int>()))
              .Returns(Task.FromResult(ResponseGeneric.Create(GetOrderNull)));

            var response = await handler.IsValid(GetWishListRequest);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task Successfull_Validations()
        {
            var response = await handler.IsValid(GetWishListRequest);
            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        public async Task Successfull_Execute()
        {
            var response = await handler.Execute(GetWishListRequest);
            Assert.IsTrue(response.Success);
        }

        #endregion
    }
}
