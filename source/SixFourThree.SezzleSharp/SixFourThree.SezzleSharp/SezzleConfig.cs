using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
        /// The URL we should redirect the customer to in the case of a cancellation
        /// </summary>
        /// <example>
        ///     https://yourdomain.com/checkout/cancel
        /// </example>
        public string CheckoutCancelUrl { get; set; }

        /// <summary>
        /// The URL we should redirect to in the case of a successful checkout
        /// </summary>
        /// <example>
        ///     https://yourdomain.com/checkout/complete
        /// </example>
        public string CheckoutCompleteUrl { get; set; }

        /// <summary>
        /// A URL for Sezzle to send our webhooks to.
        /// </summary>
        /// <example>
        ///     https://yourdomain.com/order/webhook
        /// </example>
        public string WebhookUrl { get; set; }


        /// <summary>
        /// Whether or not to use the sandbox API
        /// </summary>
        public bool UseSandbox { get; set; }

        /// <summary>
        /// Default serializer settings
        /// </summary>
        public static JsonSerializerSettings DefaultSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            Formatting = Formatting.Indented
        };

        public SezzleConfig(string apiPublicKey, string apiPrivateKey, bool useSandbox = false)
        {
            ApiUrl = useSandbox ? SandboxApiUrlDefault : ApiUrlDefault;
            ApiPublicKey = apiPublicKey;
            ApiPrivateKey = apiPrivateKey;            
        }
    }
}
