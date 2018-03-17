using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using SixFourThree.SezzleSharp.Models.Responses;

namespace SixFourThree.SezzleSharp.Endpoints
{
    /// <summary>
    /// Base class for Apis
    /// </summary>
    public abstract class SezzleApi
    {
        public SezzleConfig SezzleConfig { get; private set; }
        
        public AuthResponse AuthResponse { get; private set; }

        protected HttpClient Client { get; private set; }
        
        internal SezzleApi(string endpoint, SezzleConfig sezzleConfig) : this(endpoint, sezzleConfig, null) { }
        
        protected SezzleApi(string endpoint, SezzleConfig sezzleConfig, AuthResponse authResponse)
        {
            SezzleConfig = sezzleConfig;
            AuthResponse = authResponse;

            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip |
                                                 DecompressionMethods.Deflate;
            }

            Client = new HttpClient(handler) { BaseAddress = new Uri(new Uri(SezzleConfig.ApiUrl), endpoint) };
        }

        /// <summary>
        /// Asserts if the user is authenticated.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">You are not authenticated</exception>
        protected void AssertIsAuthenticated()
        {
            if (AuthResponse == null || String.IsNullOrWhiteSpace(AuthResponse.Token))
            {
                throw new InvalidOperationException("You are not authenticated");
            }
        }

        /// <summary>
        /// Asserts if the token is expired.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Token is expired</exception>
        protected void AssertTokenExpired()
        {
            var now = DateTime.UtcNow;

            if (AuthResponse != null && AuthResponse.ExpirationDate < now)
            {
                throw new InvalidOperationException("Token is expired");
            }
        }

        internal HttpRequestMessage Request(string fragment, HttpMethod method)
        {
            var request = new HttpRequestMessage(method, new Uri(Client.BaseAddress, fragment));

            AddAuth(request);
            
            return request;
        }        

        internal HttpRequestMessage Request(string fragment)
        {
            return Request(fragment, HttpMethod.Get);
        }

        protected virtual HttpRequestMessage AddAuth(HttpRequestMessage request)
        {
            if (AuthResponse != null)
            {
                request.Headers.Add("Authorization", $"Bearer {AuthResponse.Token}");
            }

            return request;
        }
    }
}
