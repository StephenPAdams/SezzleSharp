using SixFourThree.SezzleSharp.Models.Common;

namespace SixFourThree.SezzleSharp.Models.Orders
{
    public class OrderRefundRequest : OrderRefundRequestBody
    {
        /// <summary>
        /// ReferenceId for the RefundAsync.  This is not the 
        /// </summary>
        public string OrderReferenceId { get; set; }
    }
}
