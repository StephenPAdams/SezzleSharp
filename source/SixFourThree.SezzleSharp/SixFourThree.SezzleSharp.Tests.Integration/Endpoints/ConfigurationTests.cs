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
    public class ConfigurationTests : BaseTests
    {
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
