using System.Text.RegularExpressions;

namespace facebook_messages_analyser.Services {
    public static class StringSanitiser{
        public static string RemoveUnescapedUnicode(string str){
            var result = Regex.Replace(str, @"[^\u0000-\u007F]", string.Empty); 
            return result;
        }
    }
}