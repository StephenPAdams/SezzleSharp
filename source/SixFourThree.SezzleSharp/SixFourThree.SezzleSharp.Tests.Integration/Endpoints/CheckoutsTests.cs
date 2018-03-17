using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using SixFourThree.SezzleSharp.Endpoints;
using SixFourThree.SezzleSharp.Models;
using SixFourThree.SezzleSharp.Models.Responses;
using SixFourThree.SezzleSharp.Tests.Integration.Helpers;

namespace SixFourThree.SezzleSharp.Tests.Integration.Endpoints
{
    [TestFixture]
    public class CheckoutsTests
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

        [Test]
        public async Task CanCreateACheckoutWithoutMerchantCompletes()
        {
            var checkouts = new Checkouts(SezzleConfig, AuthResponse);

            var checkout = new Checkout();

            checkout.AmountInCents = 1000;
            checkout.CurrencyCode = "USD";
            checkout.OrderDescription = "Test checkout";
            checkout.OrderReferenceId = DateTime.UtcNow.Ticks.ToString();
            checkout.CheckoutCancelUrl = "https://test.sezzle.com/cancel";
            checkout.CheckoutCompleteUrl = "https://test.sezzle.com/complete";
            checkout.CustomerDetails = new CustomerDetails("TestFirst", "TestLast", "test@sezzle.com");
            checkout.BillingAddress = new Address("TestFirst TestLast", "1516 W. Lake St", "Minneapolis", "MN", "55408", "US");
            checkout.ShippingAddress = new Address("TestFirst TestLast", "1516 W. Lake St", "Minneapolis", "MN", "55408", "US");

            var checkoutResponse = await checkouts.Post(checkout);

            Assert.IsNotNull(checkoutResponse);

            Console.WriteLine(JsonConvert.SerializeObject(checkoutResponse, Formatting.Indented));

            Assert.IsNotNull(checkoutResponse.CheckoutUrl);
            Assert.IsNotEmpty(checkoutResponse.CheckoutUrl);
            Assert.AreNotEqual(400, checkoutResponse.Status);
        }

        [Test]
        public async Task CanCreateACheckoutWithMerchantCompletes()
        {
            var checkouts = new Checkouts(SezzleConfig, AuthResponse);

            var checkout = new Checkout();
            var orderReferenceId = DateTime.UtcNow.Ticks.ToString();

            checkout.AmountInCents = 1000;
            checkout.CurrencyCode = "USD";
            checkout.OrderDescription = "Test checkout";
            checkout.OrderReferenceId = orderReferenceId;
            checkout.CheckoutCancelUrl = "https://test.sezzle.com/cancel";
            checkout.CheckoutCompleteUrl = "https://test.sezzle.com/complete";
            checkout.CustomerDetails = new CustomerDetails("TestFirst", "TestLast", "test@sezzle.com");
            checkout.BillingAddress = new Address("TestFirst TestLast", "1516 W. Lake St", "Minneapolis", "MN", "55408", "US");
            checkout.ShippingAddress = new Address("TestFirst TestLast", "1516 W. Lake St", "Minneapolis", "MN", "55408", "US");
            checkout.MerchantCompletes = true;

            var checkoutResponse = await checkouts.Post(checkout);

            Assert.IsNotNull(checkoutResponse);

            Console.WriteLine(JsonConvert.SerializeObject(checkoutResponse, Formatting.Indented));

            Assert.IsNotNull(checkoutResponse.CheckoutUrl);
            Assert.IsNotEmpty(checkoutResponse.CheckoutUrl);
            Assert.AreNotEqual(400, checkoutResponse.Status);
            
            // Now let's try completing it
            var completeResponse = await checkouts.Complete(orderReferenceId);

            Assert.IsNotNull(completeResponse);

            Console.WriteLine(JsonConvert.SerializeObject(completeResponse, Formatting.Indented));

            Assert.AreEqual(200, completeResponse.Status);
        }
    }
}
