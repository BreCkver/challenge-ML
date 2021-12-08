using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Response;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Order.API.Host.Extensions
{
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

    }
}