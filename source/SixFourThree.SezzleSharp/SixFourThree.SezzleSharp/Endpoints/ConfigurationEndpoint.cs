using System.Threading.Tasks;
using SixFourThree.SezzleSharp.Configuration;
using SixFourThree.SezzleSharp.Extensions;
using SixFourThree.SezzleSharp.Interfaces;
using SixFourThree.SezzleSharp.Models.Configuration;

namespace SixFourThree.SezzleSharp.Endpoints
{
    public class ConfigurationEndpoint
    {
        private BaseConfiguration _baseConfiguration;
        private IAuthenticationEndpoint _authenticationEndpoint;
        private ISezzleHttpClient _sezzleHttpClient;

        public ConfigurationEndpoint(BaseConfiguration baseConfiguration, IAuthenticationEndpoint authenticationProvider, ISezzleHttpClient sezzleHttpClient)
        {
            _baseConfiguration = baseConfiguration;
            _authenticationEndpoint = authenticationProvider;
            _sezzleHttpClient = sezzleHttpClient;
        }

        /// <summary>
        /// At this time, Sezzle only allows configuration of the URL that we send our webhooks to.
        /// </summary>
        /// <remarks>
        ///     https://gateway.sezzle.com/v1/configuration
        /// </remarks>
        /// <returns></returns>
        public async Task<ConfigurationUpdateResponse> UpdateAsync(ConfigurationUpdateRequest configuration)
        {
            var tokenTask =  _authenticationEndpoint.CreateTokenAsync();

            var requestUrl = UrlStringExtensions.FormatRequestUrl(_baseConfiguration.ApiUrl, "/configuration");

            var token = await (tokenTask);
            var response = await _sezzleHttpClient.PostAsync<ConfigurationUpdateRequest, ConfigurationUpdateResponse>(token.Token, requestUrl, configuration);

            return response;
        }
    }
}
