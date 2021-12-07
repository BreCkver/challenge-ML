using System.Linq;
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
                Name = "My first WishList",
                Status = Shared.Entities.Enums.EnumOrderStatus.New
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
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }
        [TestMethod]
        public async Task Failure_Validations_RequestWLException()
        {
            orderRepository.Setup(x => x.GetOrder(It.IsAny<OrderDTO>(), It.IsAny<int>()))
               .Returns(Task.FromResult(ResponseGeneric.Create(orderNull, false)));

            var response = await handler.IsValid(GetWishListRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestUserNull()
        {
            userRepository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                 .Returns(Task.FromResult(ResponseGeneric.Create(GetUserNull)));

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
        public async Task Successfull_Created_WithList()
        {
            var response = await handler.Execute(GetWishListRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsTrue(response.Success, errors);
        }

        [TestMethod]
        public async Task Failure_Created_WithListException()
        {
            orderRepository.Setup(x => x.Insert(It.IsAny<OrderDTO>(), It.IsAny<int>()))
                .Returns(Task.FromResult(ResponseGeneric.Create(GetOrderExists, false)));
            var response = await handler.Execute(GetWishListRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }

        [TestMethod]
        public async Task Failure_Created_WithListValidation()
        {
            userRepository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                 .Returns(Task.FromResult(ResponseGeneric.Create(GetUserNull)));

            var response = await handler.Execute(GetWishListRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }

        #endregion
    }
}
