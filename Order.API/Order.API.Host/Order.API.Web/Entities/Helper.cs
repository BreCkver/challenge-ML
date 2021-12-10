using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Order.API.Web.Entities
{
    public static class Helper
    {
        public static string Process(string directori, string controller) => string.Format(CultureInfo.CurrentCulture, "{0}/api/{1}", directori, controller);

        public static string WebApiClient() => ConfigurationManager.AppSettings["webApiUri"] ?? string.Empty;

        public static  HttpClient ConfigureClient()
        {
            var productClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(5)
            };
            productClient.DefaultRequestHeaders.Accept.Clear();
            productClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            productClient.DefaultRequestHeaders.Add("Api-Version", "1");
            return productClient;
        }
    }
}