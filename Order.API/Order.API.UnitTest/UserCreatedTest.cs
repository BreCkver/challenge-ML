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
    public class UserCreatedTest
    {
        #region "  Vars Defined  "

        private UserCreatedHandler handler;
        private Mock<IUserRepository> repository;

        #endregion

        #region " Methods Initials  "

        [TestInitialize]
        public void Initialize()
        {
            repository = new Mock<IUserRepository>();
            handler = new UserCreatedHandler(repository.Object);
            Setup();
        }

        public void Setup()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                .Returns(Task.FromResult(ResponseGeneric.Create(GetUserEmpty)));
            repository.Setup(x => x.Insert(It.IsAny<UserDTO>()))
                .Returns(Task.FromResult(ResponseGeneric.Create(GetUser)));
        }

        #endregion

        #region "  Properties  "

        public UserDTO GetUserEmpty;

        public UserDTO GetUser = new UserDTO
        {
            Identifier = 123,
            UserName = GetUserRequest.UserName,
            Token = GetUserRequest.Password
        };

    
        public static UserRequest GetUserRequestPasswordDifferent => new UserRequest
        {
            UserName = "Jaime",
            Password = "PassWord",
            PasswordConfirm = "Error",
            StatusIdentifier = (int)Shared.Entities.Enums.EnumUserStatus.New
        };
        public static UserRequest GetUserRequest => new UserRequest
        {
            UserName = "Jaime",
            Password = "PassWord",
            PasswordConfirm = "PassWord",
            StatusIdentifier = (int)Shared.Entities.Enums.EnumUserStatus.New
        };

        #endregion

        #region "  Test Methods  "

        [TestMethod]
        public async Task Failure_Validations_UserExists()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
               .Returns(Task.FromResult(ResponseGeneric.Create(GetUser)));

            var response = await handler.IsValid(GetUserRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }


        [TestMethod]
        public async Task Failure_Insert_User()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUser)));

            var response = await handler.Execute(GetUserRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }

        [TestMethod]
        public async Task Failure_Validations_PassWordDifferent()
        {
            var response = await handler.Execute(GetUserRequestPasswordDifferent);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }

        [TestMethod]
        public async Task Successfull_Validations()
        {
            var response = await handler.IsValid(GetUserRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsTrue(response.Success, errors);
        }

        [TestMethod]
        public async Task Successfull_Insert_User()
        {
            var response = await handler.Execute(GetUserRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsTrue(response.Success, errors);
        }

        #endregion
    }
}
