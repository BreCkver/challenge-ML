using Order.API.Host.Extensions;
using Order.API.Host.Factory;
using Order.API.Shared.Entities.Request;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Order.API.Host.Controllers
{
    [Authorize]
    public class OrderDetailController : ApiController
    {
        [HttpGet]
        [Route("api/order/user/detail")]
        public async Task<HttpResponseMessage> WishListDetailGetFilterList([FromUri] int orderIdentifier, int userIdentifier)
        {
            var request = new WishListDetailRequest { User = new Shared.Entities.UserDTO { Identifier = userIdentifier }, WishList = new Shared.Entities.WishListDTO { Identifier = orderIdentifier } };
            var handler = new WishListDetailFactory().Create(request);
            var validate = await handler.Value.IsValid(request);
            if (validate.Failure)
            {
                //Pendiente registrar en log
                return Request.CreateResponse(validate.ToStatusCode(HttpMethod.Get), validate.ToResponse());
            }

            var result = await handler.Value.Execute(request);
            return Request.CreateResponse(result.ToStatusCode(HttpMethod.Get), result.ToResponse());
        }


        [HttpPost]
        [Route("api/order/detail")]
        public async Task<HttpResponseMessage> WishListDetailAdd([FromBody] WishListDetailRequest request)
        {
            var handler = new WishListDetailFactory().Create(request);
            var validate = await handler.Value.IsValid(request);
            if (validate.Failure)
            {
                //Pendiente registrar en log
                return Request.CreateResponse(validate.ToStatusCode(HttpMethod.Post), validate.ToResponse());
            }

            var result = await handler.Value.Execute(request);
            return Request.CreateResponse(result.ToStatusCode(HttpMethod.Post), result.ToResponse());
        }

        [HttpPut]
        [Route("api/order/detail/action/delete")]
        public async Task<HttpResponseMessage> WishListUpdate([FromBody] WishListDetailRequest request)
        {
            var handler = new WishListDetailFactory().Create(request);
            var validate = await handler.Value.IsValid(request);
            if (validate.Failure)
            {
                //Pendiente registrar en log
                return Request.CreateResponse(validate.ToStatusCode(HttpMethod.Put), validate.ToResponse());
            }

            var result = await handler.Value.Execute(request);
            return Request.CreateResponse(result.ToStatusCode(HttpMethod.Put), result.ToResponse());
        }
    }
}
