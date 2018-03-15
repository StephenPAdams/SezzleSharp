using System;
using System.Collections.Generic;
using System.Text;

namespace SixFourThree.SezzleSharp.Models.Responses
{
    public class CheckoutCompleteResponse : Response
    {
        // TODO: Look into how to have a specific response body for success (200) vs 409 (checkout expired)
        // If you pass 'true’ to 'merchant_completes’ in our Create Checkout flow, then you must call our Complete Checkout endpoint.
        // For some checkouts, a merchant may need to have the user return to their site for additional steps before completing the purchase.If this is the case, the order completion endpoint is used to complete the Checkout with Sezzle.From the time the user is redirected back to the Merchant’s site, you must make the request to complete the checkout within 30 minutes, or the checkout will be cancelled by Sezzle. If the checkout has expired, we will return the rejection response on the right, with a Status 409
        // There are two non-error responses expected.Either an HTTP 200 (currently no response body) or a rejection message.
    }
}
