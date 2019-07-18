using System.Threading.Tasks;
using SixFourThree.SezzleSharp.Configuration;
using SixFourThree.SezzleSharp.Extensions;
using SixFourThree.SezzleSharp.Interfaces;
using SixFourThree.SezzleSharp.Models.Checkouts;

namespace SixFourThree.SezzleSharp.Endpoints
{
    public class CheckoutEndpoint
    {
        private BaseConfiguration _baseConfiguration;
        private IAuthenticationEndpoint _authenticationEndpoint;
        private ISezzleHttpClient _sezzleHttpClient;

        private string GetCheckoutsBaseUrl()
        {
            return UrlStringExtensions.FormatRequestUrl(_baseConfiguration.ApiUrl, "/checkouts");
        }

        public CheckoutEndpoint(BaseConfiguration baseConfiguration, IAuthenticationEndpoint authenticationProvider, ISezzleHttpClient sezzleHttpClient)
        {
            _baseConfiguration = baseConfiguration;
            _authenticationEndpoint = authenticationProvider;
            _sezzleHttpClient = sezzleHttpClient;
        }

        /// <summary>
        /// This createCheckoutRequest endpoint creates a createCheckoutRequest in our system, and returns the URL that you should redirect the user to. We suggest you provide as much optional information about the user as you have available, since this will speed up our createCheckoutRequest process and increase conversion.
        /// Sezzle is able to handle the entire createCheckoutRequest process after a CreateCheckoutRequest has been provided.However, if your flow requires that the user confirm their createCheckoutRequest on your site after being approved by Sezzle, you may include the merchant_completes parameter with the createCheckoutRequest request.In this flow, Sezzle will not complete the order unless you make a createCheckoutRequest completion request.
        /// </summary>
        /// <remarks>
        ///     https://gateway.sezzle.com/v1/checkouts
        /// </remarks>
        /// <param name="createCheckoutRequest"></param>
        /// <returns></returns>
        public async Task<CreateCheckoutResponse> CreateAsync(CreateCheckoutRequest createCheckoutRequest)
        {
            var tokenTask =  _authenticationEndpoint.CreateTokenAsync();

            //create url and request object
            var requestUrl = GetCheckoutsBaseUrl();

            var token = await tokenTask;
            var response = await _sezzleHttpClient.PostAsync<CreateCheckoutRequest, CreateCheckoutResponse>(token.Token, requestUrl, createCheckoutRequest);

            return response;
        }

        /// <summary>
        /// If you pass 'true’ to 'merchant_completes’ in our Create CreateCheckoutRequest flow, then you must call our Complete CreateCheckoutRequest endpoint.
        /// For some checkouts, a merchant may need to have the user return to their site for additional steps before completing the purchase. 
        /// If this is the case, the order completion endpoint is used to complete the CreateCheckoutRequest with Sezzle.
        /// From the time the user is redirected back to the Merchant’s site, you must make the request to complete the createCheckoutRequest within 30 minutes, or the createCheckoutRequest will be 
        /// cancelled by Sezzle. If the createCheckoutRequest has expired, we will return the rejection response on the right, with a Status 409
        /// There are two non-error responses expected. Either an HTTP 200 (currently no response body) or a rejection message.
        /// </summary>
        /// <param name="completeCheckoutRequest"></param>
        /// <remarks>
        ///     https://gateway.sezzle.com/v1/checkouts/{order_reference_id}/complete
        /// </remarks>
        /// <returns></returns>
        public async Task<CompleteCheckoutResponse> CompleteAsync(CompleteCheckoutRequest completeCheckoutRequest)
        {

            var tokenTask = _authenticationEndpoint.CreateTokenAsync();

            //create url and request object
            var requestUrl = UrlStringExtensions.FormatRequestUrl(GetCheckoutsBaseUrl(), $"{completeCheckoutRequest.LocalOrderId}/complete");

            var token = await tokenTask;
            var response = await _sezzleHttpClient.PostAsync<CompleteCheckoutRequest, CompleteCheckoutResponse>(token.Token, requestUrl, null);

            return response;
        }
    }
}
