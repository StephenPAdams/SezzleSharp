using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SixFourThree.SezzleSharp.Models
{
    public class Checkout
    {
        /// <summary>
        /// The amount of the checkout
        /// </summary>
        /// <remarks>Required. Greater than 0.</remarks>
        public long AmountInCents { get; set; }

        /// <summary>
        /// The currency code of the checkout
        /// </summary>
        /// <remarks>Required</remarks>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// A user-facing description for this checkout
        /// </summary>
        /// <remarks>Required</remarks>
        public string OrderDescription { get; set; }

        /// <summary>
        /// A reference to this checkout in your systems. Your order reference ID must be unique identifier, and may only contain ‘a-Z’, '0-9’, ’-’, and ’_’.
        /// </summary>
        /// <remarks>Required</remarks>
        public string OrderReferenceId { get; set; }

        /// <summary>
        /// The URL we should redirect the customer to in the case of a cancellation
        /// </summary>
        /// <remarks>Required</remarks>
        public String CheckoutCancelUrl { get; set; }

        /// <summary>
        /// The URL we should redirect to in the case of a successful checkout
        /// </summary>
        /// <remarks>Required</remarks>
        public String CheckoutCompleteUrl { get; set; }

        /// <summary>
        /// The customer in the checkout
        /// </summary>
        public CustomerDetails CustomerDetails { get; set; }

        /// <summary>
        /// The billing address of the checkout
        /// </summary>
        /// <remarks>Required</remarks>
        public Address BillingAddress { get; set; }

        /// <summary>
        /// The shipping address of the checkout
        /// </summary>
        public Address ShippingAddress { get; set; }

        /// <summary>
        /// The items being purchased
        /// </summary>
        public List<Item> Items { get; set; }

        /// <summary>
        /// The discounts applied. Must be included in the total
        /// </summary>
        public List<PriceLineItem> Discounts { get; set; }

        /// <summary>
        /// The taxes applied to this checkout. Must be included in the total
        /// </summary>
        public Price TaxAmount { get; set; }

        /// <summary>
        /// The shipping fees applied to this checkout. Must be included in the total
        /// </summary>
        public Price ShippingAmount { get; set; }

        /// <summary>
        /// Optional flag to determine whether this checkout must be completed by the merchant.
        /// </summary>
        public bool MerchantCompletes { get; set; }
    }
}
