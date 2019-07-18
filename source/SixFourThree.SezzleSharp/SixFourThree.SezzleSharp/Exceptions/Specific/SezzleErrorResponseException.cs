using System;

namespace SixFourThree.SezzleSharp.Exceptions.Specific
{
    public class SezzleErrorResponseException : SezzleExceptionBase
    {
        public SezzleErrorResponseException() { }

        public SezzleErrorResponseException(string message) : base(message) { }

        public SezzleErrorResponseException(string message, Exception inner) : base(message, inner) { }
    }
}
