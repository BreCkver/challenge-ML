using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Order.API.Host.Controllers;
using Order.API.Shared.Entities.Request;

namespace Order.API.UnitTest.Controllers
{
    [TestClass]
    public class UserControllerTest
    {
        [TestMethod]
        public async System.Threading.Tasks.Task UserCreatedWithMockSucceesAsync()
        {
            UserController controller = new UserController();
            UserRequest request = new UserRequest();
            var resultado = await controller.UserCreate(request);
            Assert.IsTrue(resultado.StatusCode == System.Net.HttpStatusCode.OK);

        }
    }
}
 