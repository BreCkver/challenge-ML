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
                return Request.CreateResponse(validate.ToStatusCode(HttpMethod.Post), validate.ToResponse());
            }

            var result = await handler.Value.Execute(request);
            return Request.CreateResponse(result.ToStatusCode(HttpMethod.Post), result.ToResponse());
        }

        /// <summary>
        /// Retorna los detalles de un libro especifico del api
        /// </summary>
        /// <param name="userIdentifier"></param>
        /// <param name="bookId"></param>
        /// <returns>Libro especifico con mas detalles</returns>
        /// <response code="200">Ok, si la peticion se realizo correctamente</response>
        /// <response code="400">BadRequest, si algun parametro requerido no es definido o su valor es incorrecto </response>
        [HttpGet]
        [Route("api/order/detail/item")]
        public async Task<HttpResponseMessage> WishListDetailGetFilterList([FromUri] int userIdentifier, string bookId)
        {
            var request = new BookFilterRequest { 
                    User = new Shared.Entities.UserDTO { Identifier = userIdentifier }, 
                     Book = new Shared.Entities.BookDTO
                     {
                         ExternalIdentifier = bookId
                     }
                     };
            var handler = new BookFactory().CreateBookFilter();
            var validate = await handler.Value.IsValid(request);
            if (validate.Failure)
            {
                return Request.CreateResponse(validate.ToStatusCode(HttpMethod.Get), validate.ToResponse());
            }

            var result = await handler.Value.Execute(request);
            return Request.CreateResponse(result.ToStatusCode(HttpMethod.Get), result.ToResponse());
        }
    }
}
