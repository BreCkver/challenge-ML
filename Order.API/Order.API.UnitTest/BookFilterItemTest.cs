using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Order.API.Business.Contracts;
using Order.API.Business.Products;
using Order.API.Data.Agent;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Framework.Helpers;

namespace Order.API.UnitTest
{
    [TestClass]
    public class BookFilterItemTest
    {
        private BookFilterItemHandler handler;
        private Mock<IUserRepository> repository;
        private IGoogleApiService googleApiService;


        [TestInitialize]
        public void Initialize()
        {
            repository = new Mock<IUserRepository>();
            googleApiService = new GoogleApiService(Helper.GetGoogleApi());
            handler = new BookFilterItemHandler(googleApiService, repository.Object);
        }


        [TestMethod]
        public async Task Successfull_Validations()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(GetBookFilterRequest.User)));

            var response = await handler.IsValid(GetBookFilterRequest);
            Assert.IsTrue(response.Success);

        }

        [TestMethod]
        public async Task Failure_Validations_RequestBookEmpty()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(GetBookEmptyFilterRequest.User)));

            var response = await handler.IsValid(GetBookEmptyFilterRequest);
            Assert.IsFalse(response.Success);

        }

        [TestMethod]
        public async Task Failure_Validations_RequestUsertEmpty()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(GetUserEmptyFilterRequest.User)));

            var response = await handler.IsValid(GetUserEmptyFilterRequest);
            Assert.IsFalse(response.Success);

        }

        [TestMethod]
        public async Task Failure_Validations_RequestUsertNull()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(userNull)));

            var response = await handler.IsValid(GetBookFilterRequest);
            Assert.IsFalse(response.Success);

        }

        [TestMethod]
        public async Task Failure_Validations_RequestUsertNoExists()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(userNull, false)));

            var response = await handler.IsValid(GetBookFilterRequest);
            Assert.IsFalse(response.Success);

        }

        [TestMethod]
        public async Task Failure_Validations_RequestNull()
        {
            var response = await handler.IsValid(GetBookFilterRequestNull);
            Assert.IsFalse(response.Success);

        }

        [TestMethod]
        public async Task Failure_Books_FilterError()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(GetBookNoExistsFilterRequest.User)));

            var response = await handler.Execute(GetBookNoExistsFilterRequest);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task Successfull_Books_FilterById()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(GetBookFilterRequest.User)));

            var response = await handler.Execute(GetBookFilterRequest);
            Assert.IsTrue(response.Success);

        }


        public static BookFilterRequest GetBookFilterRequest => new BookFilterRequest
        {
            Book = new BookDTO
            {
                ExternalIdentifier = "gK98gXR8onwC"

            },
            User = new UserDTO
            {
                Identifier = 10
            },
        };

        public static BookFilterRequest GetBookEmptyFilterRequest => new BookFilterRequest
        {
            Book = new BookDTO
            {

            },
            User = new UserDTO
            {
                Token = "sdasda"
            },
        };

        public static BookFilterRequest GetUserEmptyFilterRequest => new BookFilterRequest
        {
            Book = new BookDTO
            {
                ExternalIdentifier = "gK98gXR8onwC"
            },
            User = new UserDTO
            {
                
            },
        };

        public static BookFilterRequest GetBookNoExistsFilterRequest => new BookFilterRequest
        {
            Book = new BookDTO
            {
                ExternalIdentifier = "gK98gXR8onwCz"

            },
            User = new UserDTO
            {
                Identifier = 10
            },
        };

        public UserDTO userNull;
        public BookFilterRequest GetBookFilterRequestNull;
    }
}
