using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Order.API.Business.Contracts;
using Order.API.Business.Products;
using Order.API.Data.Agent;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Request;

namespace Order.API.UnitTest
{
    [TestClass]
    public class BookFilterTest
    {
        private BookFilterHandler handler;
        private Mock<IUserRepository> repository;
        private IGoogleApiService googleApiService;

        [TestInitialize]
        public void Initialize()
        {
            repository = new Mock<IUserRepository>();
            googleApiService = new GoogleApiService(@"https://www.googleapis.com/books/v1/volumes");
            handler = new BookFilterHandler(googleApiService, repository.Object);
        }


        [TestMethod]
        public async Task Successfull_Validations()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(GetBookFilterRequest.User)));

            var response = await handler.IsValid(GetBookFilterRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsTrue(response.Success, errors);

        }

        [TestMethod]
        public async Task Failure_Validations_RequestBookEmpty()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(GetBookEmptyFilterRequest.User)));

            var response = await handler.IsValid(GetBookEmptyFilterRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);

        }

        [TestMethod]
        public async Task Failure_Validations_RequestUsertEmpty()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(GetUserEmptyFilterRequest.User)));

            var response = await handler.IsValid(GetUserEmptyFilterRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestUsertNoExists()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(userNull, false)));

            var response = await handler.IsValid(GetBookFilterRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);

        }


        [TestMethod]
        public async Task Failure_Validations_RequestUsertNull()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(userNull)));

            var response = await handler.IsValid(GetBookFilterRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }

        [TestMethod]
        public async Task Failure_Validations_RequestNull()
        {
            var response = await handler.IsValid(GetBookFilterRequestNull);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);

        }

        [TestMethod]
        public async Task Failure_Books_FilterError()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(GetBookNoExistsFilterRequest.User)));

            var response = await handler.Execute(GetBookNoExistsFilterRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsFalse(response.Success, errors);
        }

        [TestMethod]
        public async Task Successfull_Books_Filter()
        {
            repository.Setup(x => x.GetByUser(It.IsAny<UserDTO>()))
                  .Returns(Task.FromResult(ResponseGeneric.Create(GetBookFilterRequest.User)));

            var response = await handler.Execute(GetBookFilterRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsTrue(response.Success, errors);

        }

        public static BookFilterRequest GetBookFilterRequest => new BookFilterRequest
        {
            Book = new BookDTO
            {
                Authors = new System.Collections.Generic.List<string> { "keyes" },
                Keyword = "flowers"
            },
            User = new UserDTO
            {
               Identifier = 1,
            },
        };

        public static BookFilterRequest GetBookNoExistsFilterRequest => new BookFilterRequest
        {
            Book = new BookDTO
            {
                Authors = new System.Collections.Generic.List<string> { "Bondanel" },
                Keyword = "Aytes"
            },
            User = new UserDTO
            {
                Identifier = 1,
            },
        };

        public static BookFilterRequest GetBookEmptyFilterRequest => new BookFilterRequest
        {
            Book = new BookDTO
            {
                Authors = new System.Collections.Generic.List<string>(),
                Keyword = string.Empty
            },
            User = new UserDTO
            {
                Identifier = 1,
            },
        };

        public static BookFilterRequest GetUserEmptyFilterRequest => new BookFilterRequest
        {
            Book = new BookDTO
            {
                Authors = new System.Collections.Generic.List<string> { "keyes" },
                Keyword = "flowers"
            },
            User = new UserDTO
            {
                Identifier = null,
            },
        };

        public UserDTO userNull;
        public BookFilterRequest GetBookFilterRequestNull;
    }
}
