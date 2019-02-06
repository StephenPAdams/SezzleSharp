using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using SixFourThree.SezzleSharp.Endpoints;
using SixFourThree.SezzleSharp.Models;
using SixFourThree.SezzleSharp.Models.Responses;

namespace SixFourThree.SezzleSharp.Tests.Integration.Endpoints
{
    [TestFixture]
    public class CheckoutsTests : BaseTests
    {
        [Test]
        public async Task CanCreateACheckoutWithoutMerchantCompletes()
        {
            var checkouts = new Checkouts(SezzleConfig, AuthResponse);

            var checkout = new Checkout();
            var amount = 1000;
            var currencyCode = "USD";
            var orderReferenceId = DateTime.UtcNow.Ticks.ToString();

            checkout.AmountInCents = amount;
            checkout.CurrencyCode = currencyCode;
            checkout.OrderDescription = "Test checkout";
            checkout.OrderReferenceId = orderReferenceId;
            checkout.CheckoutCancelUrl = "https://test.sezzle.com/cancel";
            checkout.CheckoutCompleteUrl = "https://test.sezzle.com/complete";
            checkout.CustomerDetails = new CustomerDetails("TestFirst", "TestLast", "test@sezzle.com");
            checkout.BillingAddress = new Address("TestFirst TestLast", "1516 W. Lake St", "Minneapolis", "MN", "55408", "US");
            checkout.ShippingAddress = new Address("TestFirst TestLast", "1516 W. Lake St", "Minneapolis", "MN", "55408", "US");
            checkout.Items = new List<Item> {new Item("Test T-Shirt", "ABC123", 1, new Price(amount, currencyCode))};
            checkout.RequiresShippingInfo = false;

            var checkoutResponse = await checkouts.Post(checkout);

            Assert.IsNotNull(checkoutResponse);

            Console.WriteLine("Order reference id: {0}", orderReferenceId);
            Console.WriteLine(JsonConvert.SerializeObject(checkoutResponse, Formatting.Indented));

            Assert.IsNotNull(checkoutResponse.CheckoutUrl);
            Assert.IsNotEmpty(checkoutResponse.CheckoutUrl);
            Assert.AreNotEqual(400, checkoutResponse.Status);
        }

        /// <summary>
        /// This test will check to ensure that a checkout with the merchant_completes flag set to true is able to successfully create. 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// To test full end to integration for a checkout:
        /// Step 1, run CanCreateACheckoutWithMerchantCompletes
        /// Step 2, copy the orderReferenceId generated
        /// Step 3, visit the checkouts URL and finish the checkout process there
        /// Step 4, run this integration test CanCompleteACheckoutWithMerchantCompletes
        /// </remarks>
        [Test]
        public async Task CanCreateACheckoutWithMerchantCompletes()
        {
            var checkouts = new Checkouts(SezzleConfig, AuthResponse);

            var checkout = new Checkout();
            var amount = 1000;
            var currencyCode = "USD";
            var orderReferenceId = DateTime.UtcNow.Ticks.ToString();

            checkout.AmountInCents = amount;
            checkout.CurrencyCode = currencyCode;
            checkout.OrderDescription = "Test checkout";
            checkout.OrderReferenceId = orderReferenceId;
            checkout.CheckoutCancelUrl = "https://test.sezzle.com/cancel";
            checkout.CheckoutCompleteUrl = "https://test.sezzle.com/complete";
            checkout.CustomerDetails = new CustomerDetails("TestFirst", "TestLast", "test@sezzle.com");
            checkout.BillingAddress = new Address("TestFirst TestLast", "1516 W. Lake St", "Minneapolis", "MN", "55408", "US");
            checkout.ShippingAddress = new Address("TestFirst TestLast", "1516 W. Lake St", "Minneapolis", "MN", "55408", "US");
            checkout.Items = new List<Item> { new Item("Test T-Shirt", "ABC123", 1, new Price(amount, currencyCode)) };
            checkout.MerchantCompletes = true;
            checkout.RequiresShippingInfo = false;

            var checkoutResponse = await checkouts.Post(checkout);

            Assert.IsNotNull(checkoutResponse);

            Console.WriteLine("Order reference id: {0}", orderReferenceId);
            Console.WriteLine(JsonConvert.SerializeObject(checkoutResponse, Formatting.Indented));

            Assert.IsNotNull(checkoutResponse.CheckoutUrl);
            Assert.IsNotEmpty(checkoutResponse.CheckoutUrl);
            Assert.AreNotEqual(400, checkoutResponse.Status);
        }

        /// <summary>
        /// This test will check to ensure that a checkout with the merchant_completes flag set to true is able to successfully complete. However, due to the some stipulations:
        /// You can't capture a checkout unless it's been completed in Sezzle's system. This does make it difficult to fully automate a test checkout in Sezzle's system which 
        /// they're aware of and do intend to improve. 
        /// To resolve this, you'll need to actually complete the checkout in Sezzle's system by following the link in our checkout creation response. 
        /// You can sign up with your own phone number and re-use that account. At the bank login step, you can sign in with any credentials ("demo" "go" should work forever) 
        /// and you'll get approved.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// To test full end to integration for a checkout:
        /// Step 1, run CanCreateACheckoutWithMerchantCompletes
        /// Step 2, copy the orderReferenceId generated
        /// Step 3, visit the checkouts URL and finish the checkout process there
        /// Step 4, run this integration test CanCompleteACheckoutWithMerchantCompletes
        /// </remarks>
        [Test]
        public async Task CanCompleteACheckoutWithMerchantCompletes()
        {
            // TODO: Mark this test as ignored and require a parameter to send the order reference id into it
            var orderReferenceId = "636773928435687781";
            var checkouts = new Checkouts(SezzleConfig, AuthResponse);

            // Now let's try completing it
            var completeResponse = await checkouts.Complete(orderReferenceId);

            // Per Sezzle, an empty response body is success
            Assert.IsNotNull(completeResponse);
            Assert.AreEqual(HttpStatusCode.OK, completeResponse.HttpStatusCode);
        }
    }
}
