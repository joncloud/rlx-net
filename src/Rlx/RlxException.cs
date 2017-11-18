using System;

namespace Rlx
{
    public class RlxException : Exception
    {
        public RlxException()
        {
        }

        public RlxException(string message) : base(message)
        {
        }

        public RlxException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
