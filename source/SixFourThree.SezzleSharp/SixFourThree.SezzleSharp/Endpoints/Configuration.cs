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
    public class Configuration : SezzleApi
    {
        public Configuration(SezzleConfig config) : this(config, null) { }

        public Configuration(SezzleConfig config, AuthResponse auth) : base("configuration/", config, auth) { }

        public Configuration(string endpoint, SezzleConfig sezzleConfig, AuthResponse authResponse) : base(endpoint, sezzleConfig, authResponse) { }


        /// <summary>
        /// At this time, Sezzle only allows configuration of the URL that we send our webhooks to.
        /// </summary>
        /// <remarks>
        ///     https://gateway.sezzle.com/v1/configuration
        /// </remarks>
        /// <returns></returns>
        public Task<ConfigurationResponse> Post(Models.Configuration configuration)
        {
            AssertIsAuthenticated();

            // TODO: In the event that a token is expired, we want to refresh the token
            AssertTokenExpired();

            var request = Request("", HttpMethod.Post);

            // TODO: Abstract the string payload convert into StringContent to an extension method
            var stringPayload = JsonConvert.SerializeObject(configuration, SezzleConfig.DefaultSerializerSettings);

            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            request.Content = httpContent;

            return Client.ExecuteAsync<ConfigurationResponse>(request);
        }
    }
}
