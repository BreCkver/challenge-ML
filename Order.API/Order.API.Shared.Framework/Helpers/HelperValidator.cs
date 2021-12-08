using System.Text.RegularExpressions;
namespace Order.API.Shared.Framework.Helpers
{
    public static class HelperValidator
    {
        public static bool ValidateCharacters(this string text)
          => Regex.IsMatch(text, @"^[a-zA-Z0-9? ,_-]+$");

        public static bool ValidateCharactersSpecial(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;
            text = RemoveAccentsWithRegEx(text);
            return Regex.IsMatch(text, @"^[a-zA-Z0-9? ,_-]+$");
        }



        public static string RemoveAccentsWithRegEx(string inputString)
        {
            Regex replace_a_Accents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex replace_e_Accents = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex replace_i_Accents = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex replace_o_Accents = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex replace_u_Accents = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            Regex replace_n = new Regex("[ñ]", RegexOptions.Compiled);
            inputString = replace_a_Accents.Replace(inputString, "a");
            inputString = replace_e_Accents.Replace(inputString, "e");
            inputString = replace_i_Accents.Replace(inputString, "i");
            inputString = replace_o_Accents.Replace(inputString, "o");
            inputString = replace_u_Accents.Replace(inputString, "u");
            inputString = replace_n.Replace(inputString, "n");
            return inputString;
        }

    }
}
