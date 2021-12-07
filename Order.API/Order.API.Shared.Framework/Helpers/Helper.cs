using System.Configuration;

namespace Order.API.Shared.Framework.Helpers
{
    public static class Helper
    {
        public static string GetConnection() => ConfigurationManager.AppSettings["dbDev"] ?? string.Empty;
    }
}
