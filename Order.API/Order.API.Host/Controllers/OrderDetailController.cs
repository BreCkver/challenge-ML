using Order.API.Host.Extensions;
using Order.API.Host.Factory;
using Order.API.Shared.Entities.Request;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Order.API.Host.Controllers
{
    public class OrderDetailController : ApiController
    {
        [HttpGet]
        [Route("api/order/detail/all")]
        public async Task<HttpResponseMessage> WishListDetailGetFilterList([FromBody] WishListDetailRequest request)
        {
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
                return Request.CreateResponse(validate.ToStatusCode(HttpMethod.Get), validate.ToResponse());
            }

            var result = await handler.Value.Execute(request);
            return Request.CreateResponse(result.ToStatusCode(HttpMethod.Get), result.ToResponse());
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
                return Request.CreateResponse(validate.ToStatusCode(HttpMethod.Get), validate.ToResponse());
            }

            var result = await handler.Value.Execute(request);
            return Request.CreateResponse(result.ToStatusCode(HttpMethod.Get), result.ToResponse());
        }
    }
}
