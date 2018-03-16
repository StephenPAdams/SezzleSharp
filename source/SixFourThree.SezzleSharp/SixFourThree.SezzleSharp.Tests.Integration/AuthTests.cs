using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SixFourThree.SezzleSharp.Tests.Integration
{
    [TestFixture]
    public class AuthTests
    {
        [Test]
        public async Task CanGetTokenWithPublicAndPrivateKeys()
        {
            var appSettings = ConfigurationManager.AppSettings;

            var publicKey = appSettings["SezzleApiPublicKey"];
            var privateKey = appSettings["SezzleApiPrivateKey"];

            var config = new SezzleConfig(publicKey, privateKey, true);
            var auth = new Auth(config);
            var authResponse = await auth.RequestToken();

            Assert.IsNotNull(authResponse);
            Assert.IsNotEmpty(authResponse.Token);
            
            Console.WriteLine(authResponse.Token);
            Console.WriteLine(authResponse.ExpirationDate);
        }
    }
}
