using System;
using System.Collections.Generic;
using System.Text;

namespace SixFourThree.SezzleSharp.Models.Responses
{
    public class OrderDetailsResponse : Response
    {
        public string Description { get; set; }
        public long AmountInCents { get; set; }
        public long UsdAmountICents { get; set; }
        public string CurrencyCode { get; set; }
        public string ReferenceId { get; set; }
        public CustomerDetails Customer { get; set; }
        public Address ShippingAddress { get; set; }
    }
}
