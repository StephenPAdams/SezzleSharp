using System.Net;
using SixFourThree.SezzleSharp.Extensions;
using SixFourThree.SezzleSharp.Models;

namespace SixFourThree.SezzleSharp.Exceptions
{
    public class RequestAndResponseInfo
    {
        public string RequestUrl { get; set; }
        public string RequestBody { get; set; }
        public HttpStatusCode ResponseHttpStatusCode { get; set; }
        public string ResponseBody { get; set; }

        public SezzleGenericErrorResponse ResponseBodyAsSezzleGenericErrorResponse =>
            ResponseBody.AsSezzleGenericErrorResponse();
    }
}