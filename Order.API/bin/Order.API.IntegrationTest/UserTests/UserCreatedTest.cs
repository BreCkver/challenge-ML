using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Order.API.Business.Contracts;
using Order.API.Business.Person;
using Order.API.Data;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Framework.Helpers;
namespace Order.API.IntegrationTest.UserTests
{
    [TestClass]
    public class UserCreatedTest
    {
        private UserCreatedHandler handler;
        private IUserRepository repository;

        public UserRequest GetUserRequest => new UserRequest
        {
            UserName = GetUserName(),
            Password = "password",
            PasswordConfirm = "password",
            StatusIdentifier = (int)Shared.Entities.Enums.EnumUserStatus.New

        };

        [TestInitialize]
        public void Initialize()
        {
            var connectionString = Helper.GetConnection();
            repository = new UserData(connectionString);
            handler = new UserCreatedHandler(repository);
        }

        [TestMethod]
        public async Task Successfull_Insert_User()
        {
            var response = await handler.Execute(GetUserRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsTrue(response.Success, errors);
        }

        private string GetUserName()
        {
            Random random = new Random();
            int ranPrefix = random.Next(0, 10000);
            int ranSufix = random.Next(10000, 20000);
            return $"User{ranPrefix}-{ranSufix}";
        }
    }
}
