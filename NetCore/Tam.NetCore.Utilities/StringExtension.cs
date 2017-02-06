using System.Text;

namespace Tam.NetCore.Utilities
{
    public static class StringExtension
    {
        public static string GetStringUtf8(this byte[] input)
        {
            if (input != null && input.Length > 0)
            {
                return Encoding.UTF8.GetString(input);
            }
            return string.Empty;
        }
    }
}
