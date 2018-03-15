using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SixFourThree.SezzleSharp.Models
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
