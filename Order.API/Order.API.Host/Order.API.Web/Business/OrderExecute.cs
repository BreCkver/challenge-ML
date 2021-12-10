using Newtonsoft.Json;
using Order.API.Web.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Order.API.Web.Business
{
    public class OrderExecute
    {
        private readonly HttpClient client;

        public OrderExecute(string token)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public ResponseDTO<WishListResponse> ExecutePost(WishListRequest request, string controller)
        {
            try
            {
                var response = client.PostAsync(Helper.Process(Helper.WebApiClient(), controller), SerealizerRequest(request)).Result;
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var result = new ResponseDTO<WishListResponse>();
                    result = JsonConvert.DeserializeObject<ResponseDTO<WishListResponse>>(jsonResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    return result;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<ResponseDTO>(jsonResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    return new ResponseDTO<WishListResponse>() { Meta = result.Meta, Success = false, };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO<WishListResponse>() { Meta = new Meta { Status = string.Empty, Messages = new List<Message> { new Message { Code = "02", Text = ex.Message } } } };
            }
        }

        public ResponseDTO<WishListFilterResponse> ExecuteGet(WishListRequest request, string controller)
        {
            try
            {                
                var str = Helper.Process(Helper.WebApiClient(), controller) + "?Identifier=" + request.User.Identifier;
                var response = client.GetAsync(str, HttpCompletionOption.ResponseContentRead).Result;
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = new ResponseDTO<WishListFilterResponse>();
                    result = JsonConvert.DeserializeObject<ResponseDTO<WishListFilterResponse>>(jsonResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    return result;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<ResponseDTO>(jsonResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    return new ResponseDTO<WishListFilterResponse>() { Meta = result.Meta, Success = false, };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO<WishListFilterResponse>() { Meta = new Meta { Status = string.Empty, Messages = new List<Message> { new Message { Code = "02", Text = ex.Message } } } };
            }
        }

        public ResponseDTO<WishListResponse> ExecutePut(WishListRequest request, string controller)
        {
            try
            {
                var response = client.PutAsync(Helper.Process(Helper.WebApiClient(), controller), SerealizerRequest(request)).Result;                
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = new ResponseDTO<WishListResponse>();
                    result = JsonConvert.DeserializeObject<ResponseDTO<WishListResponse>>(jsonResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    return result;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<ResponseDTO>(jsonResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    return new ResponseDTO<WishListResponse>() { Meta = result.Meta, Success = false, };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO<WishListResponse>() { Meta = new Meta { Status = string.Empty, Messages = new List<Message> { new Message { Code = "02", Text = ex.Message } } } };
            }
        }

        private ByteArrayContent SerealizerRequest(object request)
        {
            var myContent = JsonConvert.SerializeObject(request);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }

    }
}