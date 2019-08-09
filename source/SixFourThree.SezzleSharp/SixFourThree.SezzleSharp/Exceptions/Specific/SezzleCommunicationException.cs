using System;

namespace SixFourThree.SezzleSharp.Exceptions.Specific
{
    public class SezzleCommunicationException : SezzleExceptionBase
    {
        public SezzleCommunicationException() { }

        public SezzleCommunicationException(string message) : base(message) { }

        public SezzleCommunicationException(string message, Exception inner) : base(message, inner) { }
    }
}
