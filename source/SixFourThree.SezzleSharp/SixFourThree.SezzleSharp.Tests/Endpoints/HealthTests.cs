using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SixFourThree.SezzleSharp.Endpoints;
using SixFourThree.SezzleSharp.Models.Health;

namespace SixFourThree.SezzleSharp.Tests.Endpoints
{
    [TestFixture]
    public class HealthTests : TestBase
    {
        [Test]
        public async Task TestHealth_Positive()
        {
            var client = new HealthzEndpoint(_baseConfiguration, _authenticationClient,GetDefaultSezzleHttpClient());

            //there are no parameters
            var request = new HealthCheckRequest() { };

            var response = await client.CheckHealthAsync(request);

            //if we get a response, the service is up.
            Assert.IsNotNull(response);
        }

        [Ignore("We have to ignore this test until we can trigger an exception artificially, such as a timeout. ")]
        [Test]
        public async Task TestHealth_Negative()
        {
            Action act = async () =>
            {
                //todo: inject some information that will duplicate a timeout.
                var client = new HealthzEndpoint(_baseConfiguration, _authenticationClient,
                    GetDefaultSezzleHttpClient());

                //there are no parameters
                var request = new HealthCheckRequest() { };

                var response = await client.CheckHealthAsync(request);
            };

            //assert
            act.Should().Throw<Exception>();
        }
    }
}
