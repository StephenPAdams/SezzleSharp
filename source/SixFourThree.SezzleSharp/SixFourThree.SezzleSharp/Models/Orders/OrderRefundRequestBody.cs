using System;
using System.Collections.Generic;
using System.Text;
using SixFourThree.SezzleSharp.Models.Common;

namespace SixFourThree.SezzleSharp.Models.Orders
{
   public class OrderRefundRequestBody
    {
        /// <summary>
        /// A price object. The amount to be refunded. May not exceed the total purchase amount. May not be 0. Optional if is_full_refund parameter is true.
        /// </summary>
        public Price Amount { get; set; }

        /// <summary>
        /// UUID for the RefundAsync. Must be unique to a Merchant.
        /// </summary>
        public string RefundId { get; set; }

        /// <summary>
        /// An optional reason for the refund.
        /// </summary>
        public string RefundReason { get; set; }

        /// <summary>
        /// An optional amount and currency override. If used, full order amount will be refunded.
        /// </summary>
        public bool IsFullRefund { get; set; }
    }
}
