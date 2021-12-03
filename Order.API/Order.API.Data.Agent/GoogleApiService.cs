using Newtonsoft.Json;
using Order.API.Business.Contracts;
using Order.API.Business.Contracts.Error;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Constants;
using Order.API.Shared.Entities.External;
using Order.API.Shared.Entities.Response;
using Order.API.Shared.Framework.Helpers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Order.API.Data.Agent
{
    public class GoogleApiService : IGoogleApiService
    {
        private readonly string baseUrl;
        private readonly HttpClient client;
        public GoogleApiService(string baseUrl)
        {
            this.baseUrl = baseUrl;
            client = new HttpClient();
        }

        public async Task<ResponseGeneric<BookExtendedDTO>> GetBookById(string externalIdentifier)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var requestUrl = $"{baseUrl}{externalIdentifier}";
            var response = client.GetAsync(requestUrl).Result;
            if (response == null)
            {
                return ResponseGeneric.CreateError<BookExtendedDTO>(new Error(ErrorCode.API_FAILED, $"{ErrorMessage.API_FAILED}, result is null or empty, url: {baseUrl}", ErrorType.TECHNICAL));
            }
            if (!response.IsSuccessStatusCode)
            {
                return ResponseGeneric
                .CreateError<BookExtendedDTO>(new Error(ErrorCode.API_FAILED, $"{ErrorMessage.API_FAILED} and responded with: {response.ReasonPhrase} at: {baseUrl}", ErrorType.TECHNICAL));
            }
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BookExternal>(jsonResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            if (result == null)
            {
                return ResponseGeneric
                .CreateError<BookExtendedDTO>(new Error(ErrorCode.API_FAILED, $"{ErrorMessage.API_FAILED}, failed to deserialize the object", ErrorType.TECHNICAL));
            }

            return HelperConvert.MapBookExternal(result);
        }

        public async Task<ResponseGeneric<BookResponse>> GetBooksFilter(string keyword, string title, string publisher, string author)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var requestUrl = $"{baseUrl}{QueryParameter(keyword, title, publisher, author)}&printType=books&startIndex=0&maxResults=10&projection=lite&orderBy=relevance";
            var response = client.GetAsync(requestUrl).Result;
            if (response == null)
            {
                return ResponseGeneric.CreateError<BookResponse>(new Error(ErrorCode.API_FAILED, $"{ErrorMessage.API_FAILED}, result is null or empty, url: {baseUrl}", ErrorType.TECHNICAL));
            }
            if (!response.IsSuccessStatusCode)
            {
                return ResponseGeneric
                .CreateError<BookResponse>(new Error(ErrorCode.API_FAILED, $"{ErrorMessage.API_FAILED} and responded with: {response.ReasonPhrase} at: {baseUrl}", ErrorType.TECHNICAL));
            }
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BookExternalResponse>(jsonResponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            if (result == null)
            {
                return ResponseGeneric
                .CreateError<BookResponse>(new Error(ErrorCode.API_FAILED, $"{ErrorMessage.API_FAILED}, failed to deserialize the object", ErrorType.TECHNICAL));
            }

            return HelperConvert.MapBooksExternal(result);
        }

        private string QueryParameter(string keyword, string title, string publisher, string author)
        {
            var query = new StringBuilder();
            query.Append($"?q=\"{keyword}\"");
            if (!string.IsNullOrWhiteSpace(author))
                query.Append($"+inauthor:\"{author}\"");
            if (!string.IsNullOrWhiteSpace(title))
                query.Append($"+intitle:\"{title}\"");
            if (!string.IsNullOrWhiteSpace(publisher))
                query.Append($"+inpublisher:\"{publisher}\"");

            return query.ToString();
        }
    }
}
