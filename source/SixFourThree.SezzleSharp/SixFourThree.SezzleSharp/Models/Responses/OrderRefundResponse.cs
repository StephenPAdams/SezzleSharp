using System;
using System.Collections.Generic;
using System.Text;

namespace SixFourThree.SezzleSharp.Models.Responses
{
    public class OrderRefundResponse : Response
    {
        public string RefundId { get; set; }
        public Price Amount { get; set; }
        public string RefundReason { get; set; }
    }
}
