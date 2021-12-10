using Order.API.Host.Extensions;
using Order.API.Host.Factory;
using Order.API.Shared.Entities.Request;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Order.API.Host.Controllers
{
    /// <summary>
    /// Controlador con responsabilidad de registrar, actualizar y listar los wishlist de un usuario
    /// </summary>
    [Authorize]
    public class OrderController : ApiController
    {
        /// <summary>
        /// Registra una nueva wishlist
        /// </summary>
        /// <param name="request">Tiene como obigatorios los siguientes parametros: Usuario.Identifier, WishList.Name, WishList.Status = 1</param>
        /// <returns>Entidad WishList con el identificador asignado</returns>
        /// <response code="201">Created, si se registro correctamente la nueva wishList</response>
        /// <response code="400">BadRequest, si algun parametro requerido no es definido o su valor es incorrecto </response>
        [HttpPost]
        [Route("api/order")]
        public async Task<HttpResponseMessage> WishListCreate([FromBody] WishListRequest request)
        {
            var handler = new WishListFactory().Create(request);
            var validate = await handler.Value.IsValid(request);
            if (validate.Failure)
            {
                return Request.CreateResponse(validate.ToStatusCode(HttpMethod.Post), validate.ToResponse());
            }

            var result = await handler.Value.Execute(request);
            return Request.CreateResponse(result.ToStatusCode(HttpMethod.Post), result.ToResponse());
        }


        /// <summary>
        /// Recupera los wishList asociados al usuario
        /// </summary>
        /// <param name="identifier">Identificador del usuario</param>
        /// <returns>Una lista de wishList asociadas al usuario</returns>
        /// <response code="200">Ok, si la peticion se realizo correctamente</response>
        /// <response code="400">BadRequest, si algun parametro requerido no es definido o su valor es incorrecto </response>
        [HttpGet]       
        [Route("api/order/user")]
        public async Task<HttpResponseMessage> WishListGetFilterList([FromUri] int identifier)
        {
            var handler = new WishListFactory().CreateFilter();
            var request = new WishListRequest { User = new Shared.Entities.UserDTO { Identifier = identifier }, };
            var validate = await handler.Value.IsValid(request);
            if (validate.Failure)
            {
                return Request.CreateResponse(validate.ToStatusCode(HttpMethod.Get), validate.ToResponse());
            }

            var result = await handler.Value.Execute(request);
            return Request.CreateResponse(result.ToStatusCode(HttpMethod.Get), result.ToResponse());
        }

        /// <summary>
        /// Actualiza un wishList especifico asociados al usuario, es este caso especifico lo elimina logicamente
        /// </summary>
        /// <param name="request">Tiene como obigatorios los siguientes parametros: Usuario.Identifier, WishList.Identifier, WishList.Status = 3</param>
        /// <returns>Objeto WishList con su identificador</returns>
        /// <response code="200">Ok, si la actualizacion se realizo correctamente</response>
        /// <response code="400">BadRequest, si algun parametro requerido no es definido o su valor es incorrecto </response>
        [HttpPut]        
        [Route("api/order/action/delete")]
        public async Task<HttpResponseMessage> WishListUpdate([FromBody] WishListRequest request)
        {
            var handler = new WishListFactory().Create(request);
            var validate = await handler.Value.IsValid(request);
            if (validate.Failure)
            {
                return Request.CreateResponse(validate.ToStatusCode(HttpMethod.Put), validate.ToResponse());
            }

            var result = await handler.Value.Execute(request);
            return Request.CreateResponse(result.ToStatusCode(HttpMethod.Put), result.ToResponse());
        }
    }
}
