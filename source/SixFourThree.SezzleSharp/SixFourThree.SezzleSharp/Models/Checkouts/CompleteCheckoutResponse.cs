using SixFourThree.SezzleSharp.Endpoints;
using SixFourThree.SezzleSharp.Exceptions;
using SixFourThree.SezzleSharp.Interfaces;

namespace SixFourThree.SezzleSharp.Models.Checkouts
{
    public class CompleteCheckoutResponse : ITransparentEndpointResponse
    {
        public RequestAndResponseInfo RequestAndResponseInfo { get; set; }
    }
}
