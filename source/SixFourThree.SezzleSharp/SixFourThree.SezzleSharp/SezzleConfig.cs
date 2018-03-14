using System;
using System.Collections.Generic;
using System.Text;

namespace SixFourThree.SezzleSharp
{
    public class SezzleConfig
    {
        private const string ApiUrlDefault = "https://gateway.sezzle.com/v1/";
        
        /// <summary>
        /// Base API URL
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// Scoped API public key generated from merchant dashboard
        /// </summary>
        public string ApiPublicKey { get; set; }

        /// <summary>
        /// Scoped API private key generated from merchant dashboard
        /// </summary>
        public string ApiPrivateKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SezzleConfig"/> class.
        /// </summary>
        public SezzleConfig(string apiPublicKey, string apiPrivateKey) : this(apiPublicKey, apiPrivateKey, ApiUrlDefault)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SezzleConfig"/> class.
        /// </summary>
        /// <param name="apiUri">The API URI.</param>
        /// <param name="apiPublicKey">Scoped API public key</param>
        /// <param name="apiPrivateKey">Scoped API private key</param>
        public SezzleConfig(string apiPublicKey, string apiPrivateKey, string apiUrl)
        {
            ApiUrl = apiUrl;
            ApiPublicKey = apiPublicKey;
            ApiPrivateKey = apiPrivateKey;
        }
    }
}
