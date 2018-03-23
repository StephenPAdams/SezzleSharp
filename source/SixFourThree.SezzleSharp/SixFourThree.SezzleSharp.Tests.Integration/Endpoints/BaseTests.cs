using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SixFourThree.SezzleSharp.Models.Responses;

namespace SixFourThree.SezzleSharp.Tests.Integration.Endpoints
{
    public abstract class BaseTests
    {
        public AuthResponse AuthResponse { get; set; }
        public SezzleConfig SezzleConfig { get; set; }

        [SetUp]
        public async Task Setup()
        {
            var appSettings = ConfigurationManager.AppSettings;

            var publicKey = appSettings["SezzleApiPublicKey"];
            var privateKey = appSettings["SezzleApiPrivateKey"];

            SezzleConfig = new SezzleConfig(publicKey, privateKey, true);
            var auth = new Auth(SezzleConfig);

            AuthResponse = await auth.RequestToken();
        }
    }
}
