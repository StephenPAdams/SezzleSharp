using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SixFourThree.SezzleSharp.Models
{
    public class Item
    {
        /// <summary>
        /// The name of the item
        /// </summary>
        [JsonProperty("name")]
        public string name { get; set; }

        /// <summary>
        /// The sku identifier
        /// </summary>
        [JsonProperty("sku")]
        public string sku { get; set; }

        /// <summary>
        /// The quantity purchased
        /// </summary>
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// The price
        /// </summary>
        [JsonProperty("price")]
        public Price Price { get; set; }
    }
}
