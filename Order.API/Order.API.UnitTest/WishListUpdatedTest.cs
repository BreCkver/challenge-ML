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
    public class WishListUpdatedTest
    {

        #region "  Vars Defined  "

        private WishListUpdatedHandler handler;
        private Mock<IUserRepository> userRepository;
        private Mock<IOrderRepository> orderRepository;

        #endregion

        #region " Methods Initials  "

        [TestInitialize]
        public void Initialize()
        {
            userRepository = new Mock<IUserRepository>();
            orderRepository = new Mock<IOrderRepository>();
            handler = new WishListUpdatedHandler(userRepository.Object, orderRepository.Object);
            Setup();
        }

        public void Setup()
        {
            userRepository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                .Returns(Task.FromResult(ResponseGeneric.Create(GetUser)));

            orderRepository.Setup(x => x.GetOrder(It.IsAny<OrderDTO>(), It.IsAny<int>()))
                .Returns(Task.FromResult(ResponseGeneric.Create(GetOrderExists)));

            orderRepository.Setup(x => x.Update(It.IsAny<OrderDTO>()))
               .Returns(Task.FromResult(ResponseGeneric.Create(true)));
        }

        #endregion

        #region "  Properties  "

        public UserDTO GetUserNull;

        public UserDTO GetUser = new UserDTO
        {
            Identifier = GetWishListRequest.User.Identifier,
        };

        public static WishListRequest GetWishListRequest => new WishListRequest
        {
            User = new UserDTO
            {
                Identifier = 96711,
                Token = "Token"
            },
            WishList = new WishListDTO
            {
                Identifier = 1331012,
                Status = (int)Shared.Entities.Enums.EnumOrderStatus.Deleted
            }
        };

        public OrderDTO GetOrderExists = new OrderDTO
        {
            Identifier = 1331012,
            Name = "New Name",
            Status = (int)Shared.Entities.Enums.EnumOrderStatus.Active
        };

        #endregion

        #region "  Test Methods  "


        [TestMethod]
        public async Task Failure_Validations()
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
        public async Task Successfull_Updated()
        {
            var response = await handler.Execute(GetWishListRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsTrue(response.Success, errors);
        }

        [TestMethod]
        public async Task Failure_Updated_Exception()
        {
            orderRepository.Setup(x => x.Update(It.IsAny<OrderDTO>()))
             .Returns(Task.FromResult(ResponseGeneric.Create(true, false)));

            var response = await handler.Execute(GetWishListRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }

        [TestMethod]
        public async Task Failure_Get_WithListValidation()
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
