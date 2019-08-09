using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SixFourThree.SezzleSharp.Configuration;
using SixFourThree.SezzleSharp.Extensions;
using SixFourThree.SezzleSharp.Interfaces;
using SixFourThree.SezzleSharp.Models.Checkouts;
using SixFourThree.SezzleSharp.Models.Health;

namespace SixFourThree.SezzleSharp.Endpoints
{
    public class HealthzEndpoint
    {
        private BaseConfiguration _baseConfiguration;
        private IAuthenticationEndpoint _authenticationEndpoint;
        private ISezzleHttpClient _sezzleHttpClient;

        public HealthzEndpoint(BaseConfiguration baseConfiguration, IAuthenticationEndpoint authenticationProvider, ISezzleHttpClient sezzleHttpClient)
        {
            _baseConfiguration = baseConfiguration;
            _authenticationEndpoint = authenticationProvider;
            _sezzleHttpClient = sezzleHttpClient;
        }

        public async Task<HealthCheckResponse> CheckHealthAsync(HealthCheckRequest request)
        {
            var tokenTask = _authenticationEndpoint.CreateTokenAsync();

            //create url and request object
            var requestUrl = UrlStringExtensions.FormatRequestUrl(_baseConfiguration.ApiUrl, "/healthz");
            
            var token = await tokenTask;

            //tokens are not needed to check the health, but is done here for simplicity of code reuse.
            var response = await _sezzleHttpClient.GetAsync<HealthCheckRequest, HealthCheckResponse>(token.Token, requestUrl, request);

            return response;
        }
    }
}
