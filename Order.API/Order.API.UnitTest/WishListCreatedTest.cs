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
        #region "  Vars Defined  "


        private WishListCreatedHandler handler;
        private Mock<IUserRepository> userRepository;
        private Mock<IOrderRepository> orderRepository;

        #endregion

        #region " Methods Initials  "

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

            orderRepository.Setup(x => x.Insert(It.IsAny<OrderDTO>(), It.IsAny<int>()))
                .Returns(Task.FromResult(ResponseGeneric.Create(GetOrderExists)));
        }

        #endregion

        #region "  Properties  "


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

        public UserDTO GetUser = new UserDTO
        {
            Identifier = GetWishListRequest.User.Identifier,
        };
        public UserDTO GetUserNull;
        public OrderDTO orderNull;

        #endregion

        #region "  Test Methods  "


        [TestMethod]
        public async Task Failure_Validations_RequestWLExists()
        {
            orderRepository.Setup(x => x.GetOrder(It.IsAny<OrderDTO>(), It.IsAny<int>()))
               .Returns(Task.FromResult(ResponseGeneric.Create(GetOrderExists)));

            var response = await handler.IsValid(GetWishListRequest);
            Assert.IsFalse(response.Success);
        }
        [TestMethod]
        public async Task Failure_Validations_RequestWLException()
        {
            orderRepository.Setup(x => x.GetOrder(It.IsAny<OrderDTO>(), It.IsAny<int>()))
               .Returns(Task.FromResult(ResponseGeneric.Create(orderNull, false)));

            var response = await handler.IsValid(GetWishListRequest);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestUserNull()
        {
            userRepository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                 .Returns(Task.FromResult(ResponseGeneric.Create(GetUserNull)));

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
        public async Task Successfull_Created_WithList()
        {
            var response = await handler.Execute(GetWishListRequest);
            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        public async Task Failure_Created_WithListException()
        {
            orderRepository.Setup(x => x.Insert(It.IsAny<OrderDTO>(), It.IsAny<int>()))
                .Returns(Task.FromResult(ResponseGeneric.Create(GetOrderExists, false)));
            var response = await handler.Execute(GetWishListRequest);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task Failure_Created_WithListValidation()
        {
            userRepository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                 .Returns(Task.FromResult(ResponseGeneric.Create(GetUserNull)));

            var response = await handler.Execute(GetWishListRequest);
            Assert.IsFalse(response.Success);
        }

        #endregion
    }
}
