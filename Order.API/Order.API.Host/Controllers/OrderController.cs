using Order.API.Host.Extensions;
using Order.API.Host.Factory;
using Order.API.Shared.Entities.Request;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Order.API.Host.Controllers
{
    [Authorize]
    public class OrderController : ApiController
    {
        [HttpPost]
        [Route("api/order")]
        public async Task<HttpResponseMessage> WishListCreate([FromBody] WishListRequest request)
        {
            var handler = new WishListFactory().Create(request);
            var validate = await handler.Value.IsValid(request);
            if (validate.Failure)
            {
                //Pendiente registrar en log
                return Request.CreateResponse(validate.ToStatusCode(HttpMethod.Post), validate.ToResponse());
            }

            var result = await handler.Value.Execute(request);
            return Request.CreateResponse(result.ToStatusCode(HttpMethod.Post), result.ToResponse());
        }


        [HttpGet]       
        [Route("api/order/user")]
        public async Task<HttpResponseMessage> WishListGetFilterList([FromUri] int identifier)
        {
            var handler = new WishListFactory().CreateFilter();
            var request = new WishListRequest { User = new Shared.Entities.UserDTO { Identifier = identifier }, };
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
        [Route("api/order/action/delete")]
        public async Task<HttpResponseMessage> WishListUpdate([FromBody] WishListRequest request)
        {
            var handler = new WishListFactory().Create(request);
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
