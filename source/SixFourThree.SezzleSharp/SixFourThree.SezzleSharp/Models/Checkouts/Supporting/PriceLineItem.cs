using SixFourThree.SezzleSharp.Models.Common;

namespace SixFourThree.SezzleSharp.Models.Checkouts.Supporting
{
    public class PriceLineItem
    {
        /// <summary>
        /// Description of the price
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        public Price Amount { get; set; }
    }
}
