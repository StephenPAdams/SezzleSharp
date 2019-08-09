using System;
using System.Collections.Generic;
using System.Text;

namespace SixFourThree.SezzleSharp.Models
{
    public class Enums
    {
        /// <summary>
        /// Code of the currency, used when creating checkouts in Sezzle.  Sezzle defines these; they are just mirrored here for convenience
        /// </summary>
        public enum CheckoutCurrencyCodeEnum
        {
            /// <summary>
            /// US Dollars
            /// </summary>
            USD
        }

        /// <summary>
        /// The interpreted status of a Checkout or Order.
        /// </summary>
        public enum OrderStatusEnum
        {
            /// <summary>
            /// default value, you should never see this from sezzle and should never have to handle it.
            /// </summary>
            S0000UnknownOrInconsistent = 0,

            /// <summary>
            /// The Checkout has been created on the Sezzle side, and Sezzle is awaiting user to "complete" the checkout.
            /// </summary>
            S0010OrderCreated = 10,

            /// <summary>
            /// User has checkout out and paid, but Merchant has not "completed the checkout" so the entire transaction is not complete and funds are not available to the merchant.
            /// </summary>
            S0020UserHasCheckedOut = 20,

            /// <summary>
            /// User paid, Merchant has completed checkout (or checkout was completed automatically, which happens when [MerchantCompletesCheckout=false]).  Funds are available to merchant and the first occurence of payment has been drafted from the customer 
            /// </summary>
            S0030FundsCapturedAllDoneBoomshakalakaYippiekiyay = 30
        }
    }
}
