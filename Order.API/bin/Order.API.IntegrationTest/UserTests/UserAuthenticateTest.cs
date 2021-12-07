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
    public class UserAuthenticateTest
    {
        private UserAuthenticateHandler handler;
        private IUserRepository repository;

        public UserRequest GetUserRequest => new UserRequest
        {
            UserName = "Jaime",
            Password = "PassWord"
        };

        [TestInitialize]
        public void Initialize()
        {
            var connectionString = Helper.GetConnection();
            repository = new UserData(connectionString);
            handler = new UserAuthenticateHandler(repository);
        }

        [TestMethod]
        public async Task Successfull_Get_User()
        {
            var response = await handler.Execute(GetUserRequest);
            var errors = response.ErrorList != null ? string.Join("-", response.ErrorList.Select(e => e.Message)) : string.Empty;
            Assert.IsTrue(response.Success);
        }
    }
}
