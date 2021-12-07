using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Order.API.Business.Contracts;
using Order.API.Business.Person;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Request;

namespace Order.API.UnitTest
{
    [TestClass]
    public class UserAuthenticateTest
    {
        #region "  Vars Defined  "

        private UserAuthenticateHandler handler;
        private Mock<IUserRepository> repository;

        #endregion

        #region " Methods Initials  "

        [TestInitialize]
        public void Initialize()
        {
            repository = new Mock<IUserRepository>();
            handler = new UserAuthenticateHandler(repository.Object);
            Setup();
        }

        public void Setup()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUser)));
        }


        #endregion

        #region "  Properties  "

        public UserDTO GetUserEmpty;

        public UserDTO GetUser = new UserDTO
        {
            Identifier = 1,
            UserName = GetUserRequest.UserName,
            Password = GetUserRequest.Password
        };

        public static UserRequest GetUserRequestNull;
        public static UserRequest GetUserRequestEmpty => new UserRequest
        {
            UserName = "Jaime85",
            Password = ""

        };
        public static UserRequest GetUserRequest => new UserRequest
        {
            UserName = "Jaime85",
            Password = "12345678"
        };

        #endregion

        #region "  Test Methods  "

        [TestMethod]
        public async Task Failure_Validations_RequestNull()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUserEmpty)));


            var response = await handler.IsValid(GetUserRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }


        [TestMethod]
        public async Task Successfull_Validations_User()
        {
            var response = await handler.IsValid(GetUserRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsTrue(response.Success, errors);
        }


        [TestMethod]
        public async Task Failure_Validations_RequestEmptyException()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUserEmpty, false)));

            var response = await handler.Execute(GetUserRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }


        [TestMethod]
        public async Task Successfull_Authenticate_User()
        {
            var response = await handler.Execute(GetUserRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsTrue(response.Success, errors);
        }


        [TestMethod]
        public async Task Failure_Authenticate_User()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUser, false)));

            var response = await handler.Execute(GetUserRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }

        #endregion
    }
}
