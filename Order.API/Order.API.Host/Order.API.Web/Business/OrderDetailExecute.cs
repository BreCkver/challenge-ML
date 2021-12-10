using Newtonsoft.Json;
using Order.API.Web.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Order.API.Web.Business
{
    public class OrderDetailExecute
    {
        private readonly HttpClient client;

        public OrderDetailExecute(string token)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public ResponseDTO<WishListDetailResponse> ExecutePost(WishListDetailRequest request, string controller)
        {
            try
            {
                var response = client.PostAsync(Helper.Process(Helper.WebApiClient(), controller), SerealizerRequest(request)).Result;
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var result = new ResponseDTO<WishListDetailResponse>();
                    result = JsonConvert.DeserializeObject<ResponseDTO<WishListDetailResponse>>(jsonResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    return result;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<ResponseDTO>(jsonResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    return new ResponseDTO<WishListDetailResponse>() { Meta = result.Meta, Success = false, };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO<WishListDetailResponse>() { Meta = new Meta { Status = string.Empty, Messages = new List<Message> { new Message { Code = "02", Text = ex.Message } } } };
            }
        }

        public ResponseDTO<WishListDetailResponse> ExecuteGet(WishListDetailRequest request, string controller)
        {
            try
            {
                //var myContent = JsonConvert.SerializeObject(request);
                var str = Helper.Process(Helper.WebApiClient(), controller) + "?orderIdentifier=" + request.WishList.Identifier + "&userIdentifier=" + request.User.Identifier;
                var response = client.GetAsync(str, HttpCompletionOption.ResponseContentRead).Result;
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = new ResponseDTO<WishListDetailResponse>();
                    result = JsonConvert.DeserializeObject<ResponseDTO<WishListDetailResponse>>(jsonResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    return result;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<ResponseDTO>(jsonResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    return new ResponseDTO<WishListDetailResponse>() { Meta = result.Meta, Success = false, };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO<WishListDetailResponse>() { Meta = new Meta { Status = string.Empty, Messages = new List<Message> { new Message { Code = "02", Text = ex.Message } } } };
            }
        }

        public ResponseDTO<WishListDetailResponse> ExecutePut(WishListDetailRequest request, string controller)
        {
            try
            {
                var response = client.PutAsync(Helper.Process(Helper.WebApiClient(), controller), SerealizerRequest(request)).Result;
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = new ResponseDTO<WishListDetailResponse>();
                    result = JsonConvert.DeserializeObject<ResponseDTO<WishListDetailResponse>>(jsonResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    return result;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<ResponseDTO>(jsonResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    return new ResponseDTO<WishListDetailResponse>() { Meta = result.Meta, Success = false, };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO<WishListDetailResponse>() { Meta = new Meta { Status = string.Empty, Messages = new List<Message> { new Message { Code = "02", Text = ex.Message } } } };
            }
        }

        public ResponseDTO<BookResponse> ExecutePost(BookFilterRequest request, string controller)
        {
            try
            {                
                var response = client.PostAsync(Helper.Process(Helper.WebApiClient(), controller), SerealizerRequest(request)).Result;
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var result = new ResponseDTO<BookResponse>();
                    result = JsonConvert.DeserializeObject<ResponseDTO<BookResponse>>(jsonResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    return result;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<ResponseDTO>(jsonResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    return new ResponseDTO<BookResponse>() { Meta = result.Meta, Success = false, };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO<BookResponse>() { Meta = new Meta { Status = string.Empty, Messages = new List<Message> { new Message { Code = "02", Text = ex.Message } } } };
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