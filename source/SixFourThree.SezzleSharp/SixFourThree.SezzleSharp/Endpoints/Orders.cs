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
    public class Orders : SezzleApi
    {
        public Orders(SezzleConfig config) : this(config, null) { }

        public Orders(SezzleConfig config, AuthResponse auth) : base("orders/", config, auth) { }

        public Orders(string endpoint, SezzleConfig sezzleConfig, AuthResponse authResponse) : base(endpoint, sezzleConfig, authResponse) { }

        /// <summary>
        /// Once an order is created, you can retrieve the details of the order using this endpoint.
        /// </summary>
        /// <remarks>
        ///     https://gateway.sezzle.com/v1/orders/{order_reference_id}
        /// </remarks>
        /// <param name="orderReferenceId"></param>
        /// <param name="includeShippingInfo">If your checkout post data required us to collect shipping information from the customer, then you can request that information alongside the order details.</param>
        /// <returns></returns>
        public Task<OrderDetailsResponse> Get(string orderReferenceId, bool includeShippingInfo)
        {
            AssertIsAuthenticated();

            // TODO: In the event that a token is expired, we want to refresh the token
            AssertTokenExpired();

            Ensure.ArgumentNotNullOrEmptyString(orderReferenceId, nameof(orderReferenceId));
            
            var request = Request("{order-reference-id}", HttpMethod.Get);
            request.AddUrlSegment("order-reference-id", orderReferenceId);
            request.AddParameter("include-shipping-info", includeShippingInfo.ToLowerString());
            
            return Client.ExecuteAsync<OrderDetailsResponse>(request);
        }

        /// <summary>
        /// Sezzle allows refunds for orders either through our Merchant Dashboard or through the API. If the refund is processed through the dashboard, a webhook will be sent to your system. In either case, Sezzle allows for either partial or complete refunds. Refund amounts are relative to the order total, not the amount that has been paid by the shopper.
        /// </summary>
        /// <remarks>
        ///     https://gateway.sezzle.com/v1/orders/{order_reference_id}/refund
        /// </remarks>
        /// <param name="orderReferenceId"></param>
        /// <param name="orderRefund"></param>
        /// <returns></returns>
        public Task<OrderRefundResponse> Refund(string orderReferenceId, OrderRefund orderRefund)
        {
            AssertIsAuthenticated();

            // TODO: In the event that a token is expired, we want to refresh the token
            AssertTokenExpired();

            Ensure.ArgumentNotNullOrEmptyString(orderReferenceId, nameof(orderReferenceId));

            var request = Request("{order-reference-id}/refund", HttpMethod.Post);
            request.AddUrlSegment("order-reference-id", orderReferenceId);

            // TODO: Abstract the string payload convert into StringContent to an extension method
            var stringPayload = JsonConvert.SerializeObject(orderRefund, SezzleConfig.DefaultSerializerSettings);

            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            request.Content = httpContent;

            return Client.ExecuteAsync<OrderRefundResponse>(request);
        }
    }
}
