using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SixFourThree.SezzleSharp.Configuration;
using SixFourThree.SezzleSharp.Extensions;
using SixFourThree.SezzleSharp.Interfaces;
using SixFourThree.SezzleSharp.Models.Authentication;

namespace SixFourThree.SezzleSharp.Endpoints
{
    /// <summary>
    /// Sezzle Pay uses scoped API keys to allow access to the API. You can find/generate these keys on your merchant dashboard once you have been approved by Sezzle.
    /// Once you have a valid token, it must be used as a Header for subsequent requests to their API, using the format below.
    /// AuthenticationEndpoint: Bearer authToken
    /// </summary>
    public class AuthenticationEndpoint : IAuthenticationEndpoint
    {
        private AuthenticationConfiguration _authenticationConfiguration;
        private BaseConfiguration _baseConfiguration;
        private ISezzleHttpClient _sezzleHttpClient;

        public AuthenticationEndpoint(BaseConfiguration baseConfiguration, AuthenticationConfiguration authenticationConfiguration, ISezzleHttpClient sezzleHttpClient)
        {
            _authenticationConfiguration = authenticationConfiguration;
            _baseConfiguration = baseConfiguration;
            _sezzleHttpClient = sezzleHttpClient;
        }
        
        /// <summary>
        /// Attempts to retrieve a token by passing over the Sezzle configuration public and private keys
        /// </summary>
        /// <returns></returns>
        public async Task<AuthenticationResponse> CreateTokenAsync()
        {
            //create url and request object
            var requestUrl = UrlStringExtensions.FormatRequestUrl(_baseConfiguration.ApiUrl, "authentication");

            var authRequest = new AuthenticationRequest()
            {
                PrivateKey = _authenticationConfiguration.ApiPrivateKey,
                PublicKey = _authenticationConfiguration.ApiPublicKey
            };

            //make request
            AuthenticationResponse resp = await _sezzleHttpClient.PostAsync<AuthenticationRequest, AuthenticationResponse>(null, requestUrl, authRequest);

            return resp;
        }
    }
}
