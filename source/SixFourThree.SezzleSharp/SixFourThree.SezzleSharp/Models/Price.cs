using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SixFourThree.SezzleSharp.Models
{
    public class Price
    {
        /// <summary>
        /// Amount in cents
        /// </summary>
        public long AmountInCents { get; set; }

        /// <summary>
        /// Currency code
        /// </summary>
        public string Currency { get; set; }

        public Price(long amountInCents, string currency)
        {
            AmountInCents = amountInCents;
            Currency = currency;
        }
    }
}
