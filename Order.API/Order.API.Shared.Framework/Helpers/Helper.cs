using System.Configuration;

namespace Order.API.Shared.Framework.Helpers
{
    public static class Helper
    {
        public static string GetConnection() => ConfigurationManager.AppSettings["dbDev"] ?? string.Empty;

        public static string GetGoogleApi() => ConfigurationManager.AppSettings["googleApi"] ?? string.Empty;

        public static string GetAppSetting(string name) => ConfigurationManager.AppSettings[name] ?? string.Empty;


    }
}
