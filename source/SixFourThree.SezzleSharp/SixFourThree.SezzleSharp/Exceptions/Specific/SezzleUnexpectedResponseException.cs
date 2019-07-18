using System;

namespace SixFourThree.SezzleSharp.Exceptions.Specific
{
    public class SezzleUnexpectedResponseException : SezzleExceptionBase
    {
        public SezzleUnexpectedResponseException() { }

        public SezzleUnexpectedResponseException(string message) : base(message) { }

        public SezzleUnexpectedResponseException(string message, Exception inner) : base(message, inner) { }
    }
}
