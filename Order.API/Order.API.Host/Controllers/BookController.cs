using Order.API.Host.Extensions;
using Order.API.Host.Factory;
using Order.API.Shared.Entities.Request;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Order.API.Host.Controllers
{
    public class BookController : ApiController
    {
        [HttpGet]
        [Route("api/order/detail/search")]
        public async Task<HttpResponseMessage> WishListDetailGetFilterList([FromBody] BookFilterRequest request)
        {
            var handler = new BookFactory().Create();
            var validate = await handler.Value.IsValid(request);
            if (validate.Failure)
            {
                //Pendiente registrar en log
                return Request.CreateResponse(validate.ToStatusCode(HttpMethod.Get), validate.ToResponse());
            }

            var result = await handler.Value.Execute(request);
            return Request.CreateResponse(result.ToStatusCode(HttpMethod.Get), result.ToResponse());
        }
    }
}
