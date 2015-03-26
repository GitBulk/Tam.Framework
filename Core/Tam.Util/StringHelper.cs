using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tam.Util
{
    public static class StringHelper
    {
        static Random rand = new Random();

        /// <summary>
        /// Create a random string
        /// </summary>
        /// <param name="length">Length of random string (length must be > 1)</param>
        /// <returns>A random string</returns>
        public static string CreateRandomString(int length = 5)
        {
            if (length < 2)
            {
                throw new Exception("Length must be > 1");
            }
            string temp = "QWERTYUIOPASDFGHJKLZXCVBNM1234567890qwertyuiopasdfghjklzxcvbnm";
            var builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                int randomValue = rand.Next(temp.Length);
                builder.Append(temp[randomValue]);
            }
            return builder.ToString();
        }

        public static string RemoveAccent(this string input)
        {
            // http://quyetdo289.wordpress.com/2012/06/23/loai-bo-dau-tieng-viet-trong-c/
            var signs = new string[]
			{
				"aAeEoOuUiIdDyY",
				"áàạảãâấầậẩẫăắằặẳẵ",
				"ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
				"éèẹẻẽêếềệểễ",
				"ÉÈẸẺẼÊẾỀỆỂỄ",
				"óòọỏõôốồộổỗơớờợởỡ",
				"ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
				"úùụủũưứừựửữ",
				"ÚÙỤỦŨƯỨỪỰỬỮ",
				"íìịỉĩ",
				"ÍÌỊỈĨ",
				"đ",
				"Đ",
				"ýỳỵỷỹ",
				"ÝỲỴỶỸ"
			};
            for (int i = 1; i < signs.Length; i++)
            {
                for (int j = 0; j < signs[i].Length; j++)
                {
                    input = input.Replace(signs[i][j], signs[0][i - 1]);
                }
            }
            return input;
            ////Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            ////string strFormD = input.Normalize(System.Text.NormalizationForm.FormD);
            ////return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

            // or
            //byte[] tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(url);
            //return System.Text.Encoding.UTF8.GetString(tempBytes);
        }

        /// <summary>
        /// Generate clean url. Ex: This is url -> Thiis-is-url
        /// </summary>
        /// <param name="input">The text you want to convert</param>
        /// <returns>A clean url</returns>
        public static string Slugify(this string input)
        {
            string temp = RemoveAccent(input).ToLower().Trim();
            // remove invalid chars
            temp = Regex.Replace(temp, @"[^a-z0-9\s-]", "");

            // convert multiple spaces, hyphens into one space
            temp = Regex.Replace(temp, @"[\-\s]+", " ");

            // convert one space into hyphen
            temp = Regex.Replace(temp, @"\s", "-");

            if (temp.Length > 45)
            {
                temp = temp.Substring(0, 45);
            }
            return temp;
        }

        /// <summary>
        /// Convert a string to title case. Ex: iron man -> Iron Man
        /// </summary>
        /// <param name="input">The text you want to convert</param>
        /// <returns>A title case string</returns>
        public static string ToTitleCase(this string input)
        {
            if (input == null)
            {
                throw new Exception("Input parameter is null.");
            }
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
        }

        /// <summary>
        /// Find two or more consecutive white spaces and replace them with a single white space.
        /// </summary>
        /// <param name="input">The text you want to convert</param>
        /// <returns></returns>
        public static string OneWhiteSpace(this string input)
        {
            // \s is white space character
            // + one or more matches
            return Regex.Replace(input.Trim(), @"\s+", " ");
        }

        /// <summary>
        /// Get full qualified domain name
        /// </summary>
        /// <returns>Full qualified domain name</returns>
        public static string GetFullQualifiedDomainName()
        {
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            return (ipProperties.HostName + "." + ipProperties.DomainName).Trim('.');
        }


        /// <summary>
        /// Remove script and noscript tag.
        /// </summary>
        /// <param name="input">The text you want to remove script, noscritp tag</param>
        /// <returns>A string without scritp and noscript tag</returns>
        public static string RemoveScript(string input)
        {
            // http://www.ostree.org/code-snippet/56/extract-text-information-from-html
            string result = input;

            // . is any character
            // * is zero or more matches
            result = Regex.Replace(result, "<script.*>.*</script>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            result = Regex.Replace(result, "<noscript.*>.*</noscript>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return result;
        }

        /// <summary>
        /// Remove html comment
        /// </summary>
        /// <param name="input">The text you want to remove html comment</param>
        /// <returns>A string without html comment</returns>
        public static string RemoveHtmlComment(string input)
        {
            string result = input;
            // . is any character
            // * is zero or more matches
            return Regex.Replace(result, "<!--.*-->", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        /// <summary>
        /// Remove style tag.
        /// </summary>
        /// <param name="input">The text you want to remove style tag</param>
        /// <returns>A string without style tag</returns>
        public static string RemoveHtmlStyle(string input)
        {
            string result = input;
            // . is any character
            // * is zero or more matches
            return Regex.Replace(result, @"<style.*>.*</style>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }


        /// <summary>
        /// Check a string is ip address
        /// </summary>
        /// <param name="input">A string to check</param>
        /// <returns>True: valid ip. False: invalid ip.</returns>
        public static bool ValidateIPAddress(string input)
        {
            // \b: matches a word boundary
            // (?: ...) is everything inside the bracket won't be captured.
            // {3} is exactly 3 matches
            // \b: match the closing word boundary.
            string ipPattern = @"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";
            Match match = Regex.Match(input, ipPattern);
            if (match.Success)
            {
                return true;
            }
            return false;
        }

        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static string Truncate(this string text, int maxCharacters, string trailingText = "...")
        {
            if (string.IsNullOrWhiteSpace(text) || maxCharacters <= 0)
            {
                return text;
            }
            string result = text;
            if (text.Length >= maxCharacters)
            {
                result = text.Substring(0, maxCharacters) + trailingText;
            }
            return result;
        }

        public static string ToString(this IEnumerable<int> value)
        {
            if (value == null || value.Any() == false)
            {
                return string.Empty;
            }
            return string.Join(",", value);
        }
    }
}
