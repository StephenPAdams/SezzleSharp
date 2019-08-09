using System.Threading.Tasks;
using SixFourThree.SezzleSharp.Configuration;
using SixFourThree.SezzleSharp.Extensions;
using SixFourThree.SezzleSharp.Interfaces;
using SixFourThree.SezzleSharp.Models.Orders;

namespace SixFourThree.SezzleSharp.Endpoints
{
    public class OrdersEndpoint
    {
        private BaseConfiguration _baseConfiguration;
        private IAuthenticationEndpoint _authenticationEndpoint;
        private ISezzleHttpClient _sezzleHttpClient;


        public OrdersEndpoint(BaseConfiguration baseConfiguration, IAuthenticationEndpoint authenticationProvider, ISezzleHttpClient sezzleHttpClient)
        {
            _baseConfiguration = baseConfiguration;
            _authenticationEndpoint = authenticationProvider;
            _sezzleHttpClient = sezzleHttpClient;
        }

        /// <summary>
        /// Once an order is created, you can retrieve the details of the order using this endpoint.
        /// </summary>
        /// <remarks>
        ///     https://gateway.sezzle.com/v1/orders/{order_reference_id}
        /// </remarks>
        /// <returns></returns>
        public async Task<OrderDetailsResponse> GetDetailsAsync(OrderDetailsRequest orderDetailsRequest)
        {
            var tokenTask = _authenticationEndpoint.CreateTokenAsync();

            //build in the shipping information to true for all requests.  doesn't hurt to have it.
            var requestUrl = UrlStringExtensions.FormatRequestUrl(_baseConfiguration.ApiUrl, $"/orders/{orderDetailsRequest.OrderReferenceId}?include-shipping-info=true");

            var token = await tokenTask;
            //send null into the client; we have manually added the necessary request data to parameters and url.
            var response = await _sezzleHttpClient.GetAsync<OrderDetailsRequestBody, OrderDetailsResponse>(token.Token, requestUrl, null, null);

            return response;
        }

        /// <summary>
        /// Sezzle allows refunds for orders either through our Merchant Dashboard or through the API. If the refund is processed through the dashboard, a webhook will be sent to your system. In either case, Sezzle allows for either partial or complete refunds. RefundAsync amounts are relative to the order total, not the amount that has been paid by the shopper.
        /// </summary>
        /// <remarks>
        ///     https://gateway.sezzle.com/v1/orders/{order_reference_id}/refund
        /// </remarks>       
        /// <param name="orderRefundRequest"></param>
        /// <returns></returns>
        public async Task<OrderRefundResponse> RefundAsync(OrderRefundRequest orderRefundRequest)
        {
            var tokenTask = _authenticationEndpoint.CreateTokenAsync();

            //build in the shipping information to true for all requests.  doesn't hurt to have it.
            var requestUrl = UrlStringExtensions.FormatRequestUrl(_baseConfiguration.ApiUrl, $"/orders/{orderRefundRequest.OrderReferenceId}/refund");

            var token = await tokenTask;
            //send null into the client; we have manually added the necessary request data to parameters and url.
            var response = await _sezzleHttpClient.PostAsync<OrderRefundRequestBody, OrderRefundResponse>(token.Token, requestUrl, orderRefundRequest, null);

            return response;
        }
    }
}
