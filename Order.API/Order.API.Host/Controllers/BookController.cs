using Order.API.Host.Extensions;
using Order.API.Host.Factory;
using Order.API.Shared.Entities.Request;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Order.API.Host.Controllers
{
    /// <summary>
    /// Controlador dedicado a la busqueda de libros en una api externa
    /// </summary>
    [Authorize]
    public class BookController : ApiController
    {
        /// <summary>
        /// Obtiene una lista de libros(10) por orden de mayor relevancia con base en los filtros, Titulo, Author, Editorial, Palabra clave
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Listado de libros que cumplan con el filtro indicado</returns>
        /// <response code="201">Created, si se ejecuto de manera correcta </response>
        /// <response code="400">BadRequest, si algun parametro requerido no esta definido o su valor es incorrecto </response>
        [HttpPost]
        [Route("api/order/detail/search")]
        public async Task<HttpResponseMessage> WishListDetailGetFilterList([FromBody] BookFilterRequest request)
        {
            var handler = new BookFactory().Create();
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
