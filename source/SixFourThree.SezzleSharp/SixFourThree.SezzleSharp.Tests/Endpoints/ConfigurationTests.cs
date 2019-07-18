using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using SixFourThree.SezzleSharp.Models.Configuration;

namespace SixFourThree.SezzleSharp.Tests.Endpoints
{
    [TestFixture]
    public class ConfigurationTests : TestBase
    {
        [Test]
        public async Task CanUpdateConfiguration()
        {
            var configurationEndpoint = new SezzleSharp.Endpoints.ConfigurationEndpoint(_baseConfiguration,_authenticationClient,GetDefaultSezzleHttpClient());

            var request = new ConfigurationUpdateRequest()
            {
                WebhookUrl = "https://test.sezzle.com/orders-webhook" 
            };

            ConfigurationUpdateResponse response = await configurationEndpoint.UpdateAsync(request);

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.RequestAndResponseInfo.ResponseHttpStatusCode);
        }
    }
}
