using System;
using SixFourThree.SezzleSharp.Models.Common;

namespace SixFourThree.SezzleSharp.Models.WebHook
{
    /// <summary>
    /// Because the majority of a consumer’s checkout process happens on Sezzle’s pages, our API uses webhooks to communicate information about checkout updates, completions, or refunds to your system.
    /// We expect any response in the 200 range on submitting webhooks.
    /// </summary>
    public class OrderWebhook
    {
        /// <summary>
        /// The time (UTC) at which the Webhook was generated.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// A unique identifier for the webhook.
        /// </summary>
        public string Uuid { get; set; }

        /// <summary>
        /// The high-level category. For example, order_update
        /// </summary>
        /// <remarks>
        /// Might want to cast this to an enumeration using a JSON serializer. Currently only order_update is a possibility, but it's better than using magic strings.
        /// </remarks>
        public string Type { get; set; }

        /// <summary>
        /// The specific action. For example, order_complete or order_refund.
        /// </summary>
        /// <remarks>
        /// Might want to cast this to an enumeration using a JSON serializer. Possible values are:
        /// order_complete - The checkout was completed successfully
        /// order_refund - The order was refunded from the Sezzle Merchant Dashboard
        /// </remarks>
        public string Event { get; set; }

        /// <summary>
        /// The ID for the CreateCheckoutRequest/Order.
        /// </summary>
        public string ObjectUuid { get; set; }

        /// <summary>
        /// Unique ID for a refund. Included if the webhook event is order_refund.
        /// </summary>
        public string RefundId { get; set; }

        /// <summary>
        /// Price object. Included if the webhook event is order_refund.
        /// </summary>
        public Price RefundAmount { get; set; }
    }
}
