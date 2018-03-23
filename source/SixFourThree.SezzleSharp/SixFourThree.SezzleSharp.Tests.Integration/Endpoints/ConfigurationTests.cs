using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using SixFourThree.SezzleSharp.Models.Responses;

namespace SixFourThree.SezzleSharp.Tests.Integration.Endpoints
{
    public class ConfigurationTests
    {
        public AuthResponse AuthResponse { get; set; }
        public SezzleConfig SezzleConfig { get; set; }

        [SetUp]
        public async Task Setup()
        {
            // TODO: Make a base setup method and an inheritable class for endpoint driven tests

            var appSettings = ConfigurationManager.AppSettings;

            var publicKey = appSettings["SezzleApiPublicKey"];
            var privateKey = appSettings["SezzleApiPrivateKey"];

            SezzleConfig = new SezzleConfig(publicKey, privateKey, true);
            var auth = new Auth(SezzleConfig);

            AuthResponse = await auth.RequestToken();
        }

        [Test]
        public async Task CanUpdateConfiguration()
        {
            var configurationEndpoint = new SezzleSharp.Endpoints.Configuration(SezzleConfig, AuthResponse);

            var configuration = new Models.Configuration() { WebhookUrl = "https://test.sezzle.com/orders-webhook" };

            var configurationResponse = await configurationEndpoint.Post(configuration);

            Assert.IsNotNull(configurationResponse);
            
            Console.WriteLine(JsonConvert.SerializeObject(configurationResponse, Formatting.Indented));
            
            Assert.AreEqual(HttpStatusCode.OK, configurationResponse.HttpStatusCode);
        }
    }
}
