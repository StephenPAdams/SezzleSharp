using System;
using System.Collections.Generic;
using System.Text;

namespace SixFourThree.SezzleSharp
{
    public class SezzleConfig
    {
        private const string ApiUrlDefault = "https://gateway.sezzle.com/v1/";
        private const string SandboxApiUrlDefault = "https://sandbox.gateway.sezzle.com/v1/";
       
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
        /// Whether or not to use the sandbox API
        /// </summary>
        public bool UseSandbox { get; set; }
        
        public SezzleConfig(string apiPublicKey, string apiPrivateKey, bool useSandbox = false)
        {
            ApiUrl = useSandbox ? SandboxApiUrlDefault : ApiUrlDefault;
            ApiPublicKey = apiPublicKey;
            ApiPrivateKey = apiPrivateKey;
        }
    }
}
