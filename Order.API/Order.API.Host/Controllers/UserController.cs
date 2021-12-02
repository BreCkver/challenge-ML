using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Order.API.Host.Extensions;
using Order.API.Host.Factory;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Request;

namespace Order.API.Host.Controllers
{
    public class UserController : ApiController
    {
        [HttpPost]
        [Route("create")]
        public async Task<HttpResponseMessage> UserCreate([FromBody] UserRequest request)
        {
            var handler = new UserFactory().Create(request);
            var validate = await handler.Value.IsValid(request);
            if (validate.Failure)
            {
                //Pendiente registrar en log
                return Request.CreateResponse(validate.ToStatusCode(HttpMethod.Post), validate.ToResponse());
            }

            var result = await handler.Value.Execute(request);
            return Request.CreateResponse(result.ToStatusCode(HttpMethod.Post), result.ToResponse());
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<HttpResponseMessage> Authenticate([FromBody] UserRequest request)
        {
            var handler = new UserFactory().Create(request);
            var validate = await handler.Value.IsValid(request);
            if (validate.Failure)
            {
                //Pendiente registrar en log
                return Request.CreateResponse(validate.ToStatusCode(HttpMethod.Post), validate.ToResponse());
            }

            var result = await handler.Value.Execute(request);
            return Request.CreateResponse(result.ToStatusCode(HttpMethod.Post), result.ToResponse());
        }
    }
}
