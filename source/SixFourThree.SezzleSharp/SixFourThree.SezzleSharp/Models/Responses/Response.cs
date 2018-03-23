using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SixFourThree.SezzleSharp.Models.Responses
{
    /// <summary>
    /// Response Base class
    /// </summary>
    public abstract class Response : ErrorResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
