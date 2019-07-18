using SixFourThree.SezzleSharp.Endpoints;
using SixFourThree.SezzleSharp.Exceptions;
using SixFourThree.SezzleSharp.Interfaces;

namespace SixFourThree.SezzleSharp.Models.Checkouts
{
    public class CreateCheckoutResponse : ITransparentEndpointResponse
    {
        public string CheckoutUrl { get; set; }
        public string SezzleCheckoutId => CheckoutUrl.Split('=')[1];
        public RequestAndResponseInfo RequestAndResponseInfo { get; set; }
    }
}
