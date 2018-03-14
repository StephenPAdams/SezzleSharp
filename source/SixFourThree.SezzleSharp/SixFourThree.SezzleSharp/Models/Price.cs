using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SixFourThree.SezzleSharp.Models
{
    public class Price
    {
        [JsonProperty("amount_in_cents")]
        public Int64 AmountInCents { get; set; }
        public String Currency { get; set; }
    }
}
