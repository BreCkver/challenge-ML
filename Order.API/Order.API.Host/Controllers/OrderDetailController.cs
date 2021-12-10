using Order.API.Host.Extensions;
using Order.API.Host.Factory;
using Order.API.Shared.Entities.Request;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Order.API.Host.Controllers
{
    /// <summary>
    /// Controlador con responsabilidad de administrar los item de un wishList
    /// </summary>
    [Authorize]
    public class OrderDetailController : ApiController
    {
        /// <summary>
        /// Recupera los items asociados a una orden de un usuario especifico
        /// </summary>
        /// <param name="orderIdentifier"></param>
        /// <param name="userIdentifier"></param>
        /// <returns>Una lista  de libros asociada a la orden</returns>
        /// <response code="200">Ok, si la peticion se realizo correctamente</response>
        /// <response code="400">BadRequest, si algun parametro requerido no es definido o su valor es incorrecto </response>
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

        /// <summary>
        /// Registra un nuevo libro al WishList Especifico
        /// </summary>
        /// <param name="request">Tiene como obigatorios los siguientes parametros: Usuario.Identifier, WishList.Identifier, Book.Title, 
        /// Book.Authors, Book.ExternalIdentifier, Book.Publisher, Book.Status = 10</param>
        /// <returns>Entidad Book con el identificador asignado</returns>
        /// <response code="201">Created, si se registro correctamente la nueva wishList</response>
        /// <response code="400">BadRequest, si algun parametro requerido no es definido o su valor es incorrecto </response>
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

        /// <summary>
        /// Actualiza un libro especifico asociados a la orden, es este caso especifico lo elimina logicamente
        /// </summary>
        /// <param name="request">Tiene como obigatorios los siguientes parametros: Usuario.Identifier, WishList.Identifier, Book.Identifier, 
        /// Book.Status = 11</param>
        /// <returns>Objeto WishList con su identificador</returns>
        /// <response code="200">Ok, si la actualizacion se realizo correctamente</response>
        /// <response code="400">BadRequest, si algun parametro requerido no es definido o su valor es incorrecto </response>
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
