using System;
using SixFourThree.SezzleSharp.Endpoints;
using SixFourThree.SezzleSharp.Exceptions;
using SixFourThree.SezzleSharp.Interfaces;
using SixFourThree.SezzleSharp.Models.Checkouts.Supporting;
using SixFourThree.SezzleSharp.Models.Common;

namespace SixFourThree.SezzleSharp.Models.Orders
{
    public class OrderDetailsResponse : ITransparentEndpointResponse
    {
        /// <summary>
        /// When the "checkout was created" at Sezzle
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The time that the merchant "completed the checkout".  This is immediate if upon creating a checkout, MerchantCompletesCheckout=false. If not null, all of the funds should be available to the merchant, and Sezzle will have captured the first of the four payments from the customer.
        /// </summary>
        public DateTime? CapturedAt { get; set; }

        /// <summary>
        /// This signifies that the user has clicked "complete" within the sezzle workflow, and thus done their part to pay for the transaction.
        /// </summary>
        public DateTime? CaptureExpiration { get; set; }

        public Enums.OrderStatusEnum OrderStatus
        {
            get
            {
                return GetOrderStatus(CreatedAt, CaptureExpiration, CapturedAt); 
            }
        }

        public static Enums.OrderStatusEnum GetOrderStatus(DateTime createdAt,
            DateTime? captureExpiration, DateTime? capturedAt)
        {
            if (createdAt.Equals(DateTime.MinValue) || createdAt.Equals(DateTime.MaxValue))
            {
                return Enums.OrderStatusEnum.S0000UnknownOrInconsistent;
            }

            if (captureExpiration.HasValue)
            {
                //the flow path 
                if (capturedAt.HasValue)
                {
                    //order is complete.
                    return Enums.OrderStatusEnum.S0030FundsCapturedAllDoneBoomshakalakaYippiekiyay;
                }

                //user has checked out
                return Enums.OrderStatusEnum.S0020UserHasCheckedOut;
            }

            if (captureExpiration == null && capturedAt.HasValue)
            {
                //this is a special case we found when creating checkouts with merchantCompletes = false.  Basically, Sezzle never sets the captureExpiration (captureExpiration=null) but they automatically set the capturedAt value.  In merchantCompletes=true, the captureExpiration is set, then when the merchant compeltes checkout, the capturedAt is hydrated.
                //order is complete
                return Enums.OrderStatusEnum.S0030FundsCapturedAllDoneBoomshakalakaYippiekiyay;
            }

            //we do not go through the complexity to verify that IF capturedAt has a value and captureExpiration has a value that this is a "bad state".  We assume this never happens.

            //order has only been created
            return Enums.OrderStatusEnum.S0010OrderCreated;
        }

        public string Description { get; set; }
        public long AmountInCents { get; set; }
        public long UsdAmountInCents { get; set; }
        public Enums.CheckoutCurrencyCodeEnum CurrencyCode { get; set; }
        public string ReferenceId { get; set; }
        public CustomerDetails Customer { get; set; }
        public Address ShippingAddress { get; set; }
        public Address BillingAddress { get; set; }
        public RequestAndResponseInfo RequestAndResponseInfo { get; set; }
    }
}
