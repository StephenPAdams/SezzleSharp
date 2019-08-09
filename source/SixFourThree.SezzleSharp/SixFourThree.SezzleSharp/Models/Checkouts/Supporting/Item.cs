using SixFourThree.SezzleSharp.Models.Common;

namespace SixFourThree.SezzleSharp.Models.Checkouts.Supporting
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

        public Item(string name, string sku, int quantity, Price price)
        {
            Name = name;
            Sku = sku;
            Quantity = quantity;
            Price = price;
        }
    }
}
