using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SixFourThree.SezzleSharp.Extensions;
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
        /// <returns></returns>
        public Task<CheckoutResponse> Post(Checkout checkout)
        {
            AssertIsAuthenticated();

            // TODO: In the event that a token is expired, we want to refresh the token
            AssertTokenExpired();

            var request = Request("", HttpMethod.Post);

            // TODO: Abstract the string payload c onvert into StringContent to an extension method
            var stringPayload = JsonConvert.SerializeObject(checkout);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            request.Content = httpContent;

            return Client.ExecuteAsync<CheckoutResponse>(request);
        }
    }
}
