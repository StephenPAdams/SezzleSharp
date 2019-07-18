using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SixFourThree.SezzleSharp.Models.Common
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
        [JsonConverter(typeof(StringEnumConverter))]
        public Enums.CheckoutCurrencyCodeEnum Currency { get; set; }

        public Price()
        {

        }
    }
}
