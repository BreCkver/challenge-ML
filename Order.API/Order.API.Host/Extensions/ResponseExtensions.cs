using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Response;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.IdentityModel.Tokens;
using Order.API.Shared.Framework.Helpers;
using System.Security.Claims;
using System;

namespace Order.API.Host.Extensions
{
    /// <summary>
    /// Metodo extension con funcionalidades como convertir una respuesta y generar el token 
    /// </summary>
    public static class ResponseExtensions
    {
       /// <summary>
       /// 
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="response"></param>
       /// <returns></returns>
        public static Response<T> ToResponse<T>(this ResponseGeneric<T> response)
        {
            string status = "Succeeded";
            var messages = new List<Message>();
            if (response.Failure)
            {
                status = "Failed";
                response.ErrorList?.ForEach(error =>
                {
                    messages.Add(new Message
                    {
                        Code = error.Code,
                        Text = error.Message
                    });
                });
            }

            return new Response<T>
            {
                Data = response.Value,
                Meta = new Meta
                {
                    Status = status,
                    Messages = messages
                }
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static HttpStatusCode ToStatusCode<T>(this ResponseGeneric<T> response, HttpMethod method)
        {
            HttpStatusCode code = HttpStatusCode.OK;

            if (response.Failure)
            {
                code = HttpStatusCode.InternalServerError;
                if (response.ErrorList.Any(error => error.Code.Contains("400_")))
                    code = HttpStatusCode.BadRequest;
                if (response.ErrorList.Any(error => error.Code.Contains("401_")))
                    code = HttpStatusCode.Unauthorized;
                if (response.ErrorList.Any(error => error.Code.Contains("409_")))
                    code = HttpStatusCode.Conflict;
                return code;
            }

            var options = new Dictionary<HttpMethod, HttpStatusCode>
            {
                { HttpMethod.Get, HttpStatusCode.OK },
                { HttpMethod.Post, HttpStatusCode.Created },
                { HttpMethod.Put, HttpStatusCode.OK }
            };

            if (!options.TryGetValue(method, out code))
                return HttpStatusCode.Unused;

            return code;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static string CreatedToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(Helper.GetAppSetting("secret_key")));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) });
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: Helper.GetAppSetting("audience_token"),
                issuer: Helper.GetAppSetting("issuer_token"),
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(Helper.GetAppSetting("expire_token"))),
                signingCredentials: signingCredentials);

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtTokenString;
        }
    }
}