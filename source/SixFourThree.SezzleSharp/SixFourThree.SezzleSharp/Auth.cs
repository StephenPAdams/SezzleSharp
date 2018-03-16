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

namespace SixFourThree.SezzleSharp
{
    /// <summary>
    /// Sezzle Pay uses scoped API keys to allow access to the API. You can find/generate these keys on your merchant dashboard once you have been approved by Sezzle.
    /// Once you have a valid token, it must be used as a Header for subsequent requests to their API, using the format below.
    /// Authorization: Bearer authToken
    /// </summary>
    public class Auth
    {
        public SezzleConfig SezzleConfig { get; private set; }
        protected HttpClient Client { get; private set; }

        public Auth(SezzleConfig sezzleConfig)
        {
            SezzleConfig = sezzleConfig;
        }

        /// <summary>
        /// Attempts to retrieve a token by passing over the Sezzle configuration public and private keys
        /// </summary>
        /// <returns></returns>
        public Task<AuthResponse> RequestToken()
        {
            Ensure.ArgumentNotNullOrEmptyString(SezzleConfig.ApiPrivateKey, nameof(SezzleConfig.ApiPrivateKey));
            Ensure.ArgumentNotNullOrEmptyString(SezzleConfig.ApiPublicKey, nameof(SezzleConfig.ApiPublicKey));
            
            Client = new HttpClient() { BaseAddress = new Uri(new Uri(SezzleConfig.ApiUrl), "authentication") };

            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(Client.BaseAddress, String.Empty));

            var authentication = new Authentication() { PrivateKey = SezzleConfig.ApiPrivateKey, PublicKey =  SezzleConfig.ApiPublicKey };

            // TODO: Clean up code between Auth any of the Sezzle based endpoint classes, etc.
            // TODO: Abstract the string payload convert into StringContent to an extension method
            var stringPayload = JsonConvert.SerializeObject(authentication, SezzleConfig.DefaultSerializerSettings);

            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            request.Content = httpContent;

            return Client.ExecuteAsync<AuthResponse>(request);
        }
    }
}
