using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Order.API.Host.Extensions;
using Order.API.Host.Factory;
using Order.API.Shared.Entities.Request;

namespace Order.API.Host.Controllers
{   
    /// <summary>
    /// Controlador con responsabilidad de registra nuevos usuarios o authentificarlos
    /// </summary>
    [AllowAnonymous]
    public class UserController : ApiController
    {
        /// <summary>
        /// Registra un nuevo usuario con base en Nombre de Usuario, PassWord y Estatus (1)
        /// </summary>
        /// <param name="request">Parametros obligatorios son UserName, Password, PasswordConfirm, StatusIdentifier = 1 </param>
        /// <returns>Entidad usurio con el identificador asignado al usuario</returns>
        /// <response code="201">Created, si se registro correctamente el usuario</response>
        /// <response code="400">BadRequest, si algun parametro requerido no es definido o su valor es incorrecto </response>
        /// <response code="409">Conflic, si el nombre de usuario ya se encuentra registrado </response>
        [HttpPost]
        [Route("api/user")]
        public async Task<HttpResponseMessage> UserCreate([FromBody] UserRequest request)
        {
            var handler = new UserFactory().Create(request);
            var validate = await handler.Value.IsValid(request);
            if (validate.Failure)
            {                
                return Request.CreateResponse(validate.ToStatusCode(HttpMethod.Post), validate.ToResponse());
            }

            var result = await handler.Value.Execute(request);
            return Request.CreateResponse(result.ToStatusCode(HttpMethod.Post), result.ToResponse());
        }

        /// <summary>
        /// Authentifica a un usuario
        /// </summary>
        /// <param name="request">UserName y Password</param>
        /// <returns>Token para autentificar las peticiones posteriores, dicho token tiene una vigencia de 10 mins</returns>
        /// <response code="201">Created, si se logeo correctamente</response>
        /// <response code="400">BadRequest, si algun parametro requerido no es definido o su valor es incorrecto </response>
        /// <response code="401">Unauthorized, si el usuario no esta registrado o su contraseña no es la correcta </response>

        [HttpPost]
        [Route("api/user/authenticate")]
        public async Task<HttpResponseMessage> Authenticate([FromBody] UserRequest request)
        {
            var handler = new UserFactory().Create(request);
            var validate = await handler.Value.IsValid(request);
            if (validate.Failure)
            {
                return Request.CreateResponse(validate.ToStatusCode(HttpMethod.Post), validate.ToResponse());
            }

            var result = await handler.Value.Execute(request);
            if(result.Success)
            {
                var toker = ResponseExtensions.CreatedToken(request.UserName);
                result.Value.User.Token = toker;
            }
            return Request.CreateResponse(result.ToStatusCode(HttpMethod.Post), result.ToResponse());
        }
    }
}
