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
    public class UserCreatedTest
    {

        private UserCreatedHandler handler;
        private Mock<IUserRepository> reposotory;

        [TestInitialize]
        public void Initialize()
        {
            reposotory = new Mock<IUserRepository>();
            handler = new UserCreatedHandler(reposotory.Object);
            Setup();
        }

        public void Setup()
        {
            reposotory.Setup(x => x.Insert(It.IsAny<UserDTO>()))
                .Returns(Task.FromResult(ResponseGeneric.Create(GetUser)));
        }

        [TestMethod]
        public async Task Failure_Validations()
        {
            reposotory.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUser)));
            var response = await handler.IsValid(GetUserRequest);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestNull()
        {
            reposotory.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUserEmpty)));

            var response = await handler.IsValid(GetUserRequestNull);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestEmpty()
        {
            reposotory.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUserEmpty)));

            var response = await handler.IsValid(GetUserRequestEmpty);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task Successfull_Validations()
        {
            reposotory.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUserEmpty)));

            var response = await handler.IsValid(GetUserRequest);
            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        public async Task Successfull_Insert_User()
        {
            reposotory.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUserEmpty)));

            var response = await handler.Execute(GetUserRequest);
            Assert.IsTrue(response.Success);
        }


        [TestMethod]
        public async Task Failure_Insert_User()
        {
            reposotory.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUser)));

            var response = await handler.Execute(GetUserRequest);
            Assert.IsFalse(response.Success);
        }

        public UserDTO GetUserEmpty;

        public UserDTO GetUser = new UserDTO
        {
            UserName = GetUserRequest.UserName,
            Token = GetUserRequest.Password
        };


        public static UserCreatedRequest GetUserRequestNull;


        public static UserCreatedRequest GetUserRequestEmpty => new UserCreatedRequest
        {
            UserName = "",
            Password = "PassWord"
        };


        public static UserCreatedRequest GetUserRequest => new UserCreatedRequest
        {
            UserName = "Jaime",
            Password = "PassWord"
        };
    }
}
