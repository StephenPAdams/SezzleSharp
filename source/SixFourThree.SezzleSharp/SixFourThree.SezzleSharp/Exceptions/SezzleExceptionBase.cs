using System;
using SixFourThree.SezzleSharp.Endpoints;
using SixFourThree.SezzleSharp.Interfaces;

namespace SixFourThree.SezzleSharp.Exceptions
{
    public class SezzleExceptionBase : Exception, ITransparentEndpointResponse
    {
        public RequestAndResponseInfo RequestAndResponseInfo { get; set; }

        public SezzleExceptionBase() { }

        public SezzleExceptionBase(string message) : base(message) { }

        public SezzleExceptionBase(string message, Exception inner) : base(message, inner) { }
    }
}
