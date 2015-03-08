using System;

namespace Tam.Util
{
    public class NullOrEmptyArgumentException : Exception
    {
        public NullOrEmptyArgumentException(string argument)
            : base(string.Format("{0} is null or empty.", argument))
        {
        }
    }
}