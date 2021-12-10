using Newtonsoft.Json;
using Order.API.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Order.API.Web.Business
{
    public class UserExecute
    {
        private readonly HttpClient client;

        public UserExecute()
        {
            client = new HttpClient();
        }

        public ResponseDTO<ResponseUser> ExecutePost(UserDTO userDTO, string controller)
        {
            try
            {
                var myContent = JsonConvert.SerializeObject(userDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
                var response = client.PostAsync(Helper.Process(Helper.WebApiClient(), controller), byteContent).Result;
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var result = new ResponseDTO<ResponseUser>();
                    result = JsonConvert.DeserializeObject<ResponseDTO<ResponseUser>>(jsonResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    return result;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<ResponseDTO>(jsonResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    return new ResponseDTO<ResponseUser>() { Meta = result.Meta, Success = false, };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO<ResponseUser>() { Meta = new Meta { Status = string.Empty, Messages = new List<Message> { new Message { Code = "02", Text = ex.Message } } } };
            }
        }
    }
}