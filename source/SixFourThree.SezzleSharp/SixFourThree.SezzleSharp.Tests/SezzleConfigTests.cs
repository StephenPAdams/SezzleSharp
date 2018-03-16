using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SixFourThree.SezzleSharp.Tests
{
    [TestFixture]
    public class SezzleConfigTests
    {
        [Test]
        public void NotSettingSandboxModeShouldUseProductionUrl()
        {
            var expectedProductionUrl = "https://gateway.sezzle.com/v1/";
            var config = new SezzleConfig(String.Empty, String.Empty, false);

            Assert.AreEqual(expectedProductionUrl, config.ApiUrl);
        }

        [Test]
        public void SettingSandboxModeShouldUseSandboxUrl()
        {
            var expectedSandboxUrl = "https://sandbox.gateway.sezzle.com/v1/";
            var config = new SezzleConfig(String.Empty, String.Empty, true);

            Assert.AreEqual(expectedSandboxUrl, config.ApiUrl);
        }
    }
}
