using System.Text.RegularExpressions;
namespace Order.API.Shared.Framework.Helpers
{
    public static class HelperValidator
    {
        public static bool ValidateCharacters(this string text)
          => Regex.IsMatch(text, "^[a-zA-Z0-9? ,_-]+$");

    }
}
