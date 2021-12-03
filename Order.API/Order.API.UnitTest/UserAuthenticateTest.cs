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

        private UserAuthenticateHandler handler;
        private Mock<IUserRepository> repository;

        [TestInitialize]
        public void Initialize()
        {
            repository = new Mock<IUserRepository>();
            handler = new UserAuthenticateHandler(repository.Object);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestNull()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUserEmpty)));

            var response = await handler.IsValid(GetUserRequestNull);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestEmpty()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUserEmpty)));

            var response = await handler.IsValid(GetUserRequestEmpty);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestUsertException()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(GetUserEmpty, false)));

            var response = await handler.IsValid(GetUserRequest);
            Assert.IsFalse(response.Success);

        }

        [TestMethod]
        public async Task Successfull_Validations()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUser)));

            var response = await handler.IsValid(GetUserRequest);
            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        public async Task Successfull_Authenticate_User()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUser)));

            var response = await handler.Execute(GetUserRequest);
            Assert.IsTrue(response.Success);
        }


        [TestMethod]
        public async Task Failure_Authenticate_User()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUserEmpty)));

            var response = await handler.Execute(GetUserRequest);
            Assert.IsFalse(response.Success);
        }

        public UserDTO GetUserEmpty;

        public UserDTO GetUser = new UserDTO
        {
            UserName = GetUserRequest.User.UserName,
            Token = GetUserRequest.User.Password
        };


        public static UserRequest GetUserRequestNull;


        public static UserRequest GetUserRequestEmpty => new UserRequest
        {
            User = new UserDTO
            {
                UserName = "Jaime85",
                Password = ""
            },
        };


        public static UserRequest GetUserRequest => new UserRequest
        {
            User = new UserDTO
            {
                UserName = "Jaime85",
                Password = "12345678"
            },
        };
    }
}
