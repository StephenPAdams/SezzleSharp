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
        public string Name { get; set; }

        /// <summary>
        /// The sku identifier
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// The quantity purchased
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The price
        /// </summary>
        public Price Price { get; set; }
    }
}
