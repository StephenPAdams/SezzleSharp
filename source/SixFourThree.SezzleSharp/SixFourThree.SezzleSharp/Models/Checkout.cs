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
        [JsonProperty("amount_in_cents")]
        public long AmountInCents { get; set; }

        /// <summary>
        /// The currency code of the checkout
        /// </summary>
        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// A user-facing description for this checkout
        /// </summary>
        [JsonProperty("order_description")]
        public string OrderDescription { get; set; }

        /// <summary>
        /// A reference to this checkout in your systems. Your order reference ID must be unique identifier, and may only contain ‘a-Z’, '0-9’, ’-’, and ’_’.
        /// </summary>
        [JsonProperty("order_reference_id")]
        public string OrderReferenceId { get; set; }

        /// <summary>
        /// The URL we should redirect the customer to in the case of a cancellation
        /// </summary>
        [JsonProperty("checkout_cancel_url")]
        public String CheckoutCancelUrl { get; set; }

        /// <summary>
        /// The URL we should redirect to in the case of a successful checkout
        /// </summary>
        [JsonProperty("checkout_complete_url")]
        public String CheckoutCompleteUrl { get; set; }

        /// <summary>
        /// The customer in the checkout
        /// </summary>
        [JsonProperty("customer_details")]
        public CustomerDetails CustomerDetails { get; set; }

        /// <summary>
        /// The billing address of the checkout
        /// </summary>
        [JsonProperty("billing_address")]
        public Address BillingAddress { get; set; }

        /// <summary>
        /// The shipping address of the checkout
        /// </summary>
        [JsonProperty("shipping_address")]
        public Address ShippingAddress { get; set; }

        /// <summary>
        /// The items being purchased
        /// </summary>
        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        /// <summary>
        /// The discounts applied. Must be included in the total
        /// </summary>
        [JsonProperty("discounts")]
        public List<PriceLineItem> Discounts { get; set; }

        /// <summary>
        /// The taxes applied to this checkout. Must be included in the total
        /// </summary>
        [JsonProperty("tax_amount")]
        public Price TaxAmount { get; set; }

        /// <summary>
        /// The shipping fees applied to this checkout. Must be included in the total
        /// </summary>
        [JsonProperty("shipping_amount")]
        public Price ShippingAmount { get; set; }

        /// <summary>
        /// Optional flag to determine whether this checkout must be completed by the merchant.
        /// </summary>
        [JsonProperty("merchant_completes")]
        public bool MerchantCompletes { get; set; }
    }
}
