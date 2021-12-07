using System.Collections.Generic;
using System.Linq;
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
    public class UserBaseValidatorTest
    {
        #region "  Vars Defined  "

        private UserHandlerMock handler;
        private Mock<IUserRepository> userRepository;

        #endregion

        #region " Methods Initials  "

        [TestInitialize]
        public void Initialize()
        {
            userRepository = new Mock<IUserRepository>();
            handler = new UserHandlerMock(userRepository.Object);
            Setup();
        }

        public void Setup()
        {
            userRepository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUserEmpty)));

        }

        #endregion

        #region "  Properties  "

        public UserDTO GetUser = new UserDTO
        {
            Identifier = 123,
            UserName = GetUserRequest.UserName,
            Token = GetUserRequest.Password
        };

        public static UserRequest GetUserRequest => new UserRequest
        {
            UserName = "Jaime",
            Password = "PassWord",
            PasswordConfirm = "PassWord",
            StatusIdentifier = (int)Shared.Entities.Enums.EnumUserStatus.New
        };

        public UserDTO GetUserEmpty;
        
        public static UserRequest GetUserRequestNull;
        public static UserRequest GetUserRequestEmpty => new UserRequest
        {
            UserName = "",
            Password = ""
        };


        #endregion


        #region "  Test Methods  "

        [TestMethod]
        public async Task Failure_Validations()
        {
            userRepository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                   .Returns(Task.FromResult(ResponseGeneric.Create(GetUser)));
            
            var response = await handler.IsValid(GetUserRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestNull()
        {
            var response = await handler.IsValid(GetUserRequestNull);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }


        [TestMethod]
        public async Task Failure_Validations_RequestEmpty()
        {
            var response = await handler.IsValid(GetUserRequestEmpty);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestUsertException()
        {
            userRepository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(GetUserEmpty, false)));

            var response = await handler.IsValid(GetUserRequest);
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
