using System;

namespace Tam.Util
{
    public static class Guard
    {
        public static void ThrowIfNull(object obj, string errorMessage = "")
        {
            if (obj == null)
            {
                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = "Parameter obj is null.";
                }
                throw new ArgumentNullException(errorMessage);
            }
        }

        public static void ThrowIfNullOrWhiteSpace(string input, string errorMessage = "")
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = "Parameter input is null or white space.";
                }
                throw new ArgumentNullException(errorMessage);
            }
        }
    }
}
