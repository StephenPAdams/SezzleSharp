using SixFourThree.SezzleSharp.Exceptions;
using SixFourThree.SezzleSharp.Interfaces;
using SixFourThree.SezzleSharp.Models.Common;

namespace SixFourThree.SezzleSharp.Models.Orders
{
    public class OrderRefundResponse:ITransparentEndpointResponse
    {
        public string RefundId { get; set; }
        public Price Amount { get; set; }
        public string RefundReason { get; set; }
        public RequestAndResponseInfo RequestAndResponseInfo { get; set; }
    }
}
