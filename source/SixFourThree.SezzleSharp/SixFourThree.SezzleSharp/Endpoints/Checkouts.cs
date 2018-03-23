using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SixFourThree.SezzleSharp.Extensions;
using SixFourThree.SezzleSharp.Helpers;
using SixFourThree.SezzleSharp.Models;
using SixFourThree.SezzleSharp.Models.Responses;

namespace SixFourThree.SezzleSharp.Endpoints
{
    public class Checkouts : SezzleApi
    {
        public Checkouts(SezzleConfig config) : this(config, null) { }

        public Checkouts(SezzleConfig config, AuthResponse auth) : base("checkouts/", config, auth) { }

        public Checkouts(string endpoint, SezzleConfig sezzleConfig, AuthResponse authResponse) : base(endpoint, sezzleConfig, authResponse) { }

        /// <summary>
        /// This checkout endpoint creates a checkout in our system, and returns the URL that you should redirect the user to. We suggest you provide as much optional information about the user as you have available, since this will speed up our checkout process and increase conversion.
        /// Sezzle is able to handle the entire checkout process after a Checkout has been provided.However, if your flow requires that the user confirm their checkout on your site after being approved by Sezzle, you may include the merchant_completes parameter with the checkout request.In this flow, Sezzle will not complete the order unless you make a checkout completion request.
        /// </summary>
        /// <remarks>
        ///     https://gateway.sezzle.com/v1/checkouts
        /// </remarks>
        /// <returns></returns>
        public Task<CheckoutResponse> Post(Checkout checkout)
        {
            AssertIsAuthenticated();

            // TODO: In the event that a token is expired, we want to refresh the token
            AssertTokenExpired();

            var request = Request("", HttpMethod.Post);

            // TODO: Abstract the string payload convert into StringContent to an extension method
            var stringPayload = JsonConvert.SerializeObject(checkout, SezzleConfig.DefaultSerializerSettings);

            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            request.Content = httpContent;

            return Client.ExecuteAsync<CheckoutResponse>(request);
        }

        /// <summary>
        /// If you pass 'true’ to 'merchant_completes’ in our Create Checkout flow, then you must call our Complete Checkout endpoint.
        /// For some checkouts, a merchant may need to have the user return to their site for additional steps before completing the purchase. 
        /// If this is the case, the order completion endpoint is used to complete the Checkout with Sezzle.
        /// From the time the user is redirected back to the Merchant’s site, you must make the request to complete the checkout within 30 minutes, or the checkout will be 
        /// cancelled by Sezzle. If the checkout has expired, we will return the rejection response on the right, with a Status 409
        /// There are two non-error responses expected. Either an HTTP 200 (currently no response body) or a rejection message.
        /// </summary>
        /// <param name="orderReferenceId"></param>
        /// <remarks>
        ///     https://gateway.sezzle.com/v1/checkouts/{order_reference_id}/complete
        /// </remarks>
        /// <returns></returns>
        public Task<CheckoutCompleteResponse> Complete(string orderReferenceId)
        {
            // TODO: Perhaps go with an attribute for these calls that require authenticated and a token not to be expired?

            AssertIsAuthenticated();

            // TODO: In the event that a token is expired, we want to refresh the token
            AssertTokenExpired();

            Ensure.ArgumentNotNullOrEmptyString(orderReferenceId, nameof(orderReferenceId));

            var request = Request("{order-reference-id}/complete", HttpMethod.Post);
            request.AddUrlSegment("order-reference-id", orderReferenceId);
            
            return Client.ExecuteAsync<CheckoutCompleteResponse>(request);
        }
    }
}
