using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using SixFourThree.SezzleSharp.Models.Checkouts;

namespace SixFourThree.SezzleSharp.Tests.Endpoints
{
    [TestFixture]
    public class CheckoutsTests : TestBase
    {

        [Test]
        public async Task CanCreateACheckoutWithoutMerchantCompletes()
        {
            var checkouts = CreateCheckoutClient();
            var orderReferenceId = DateTime.UtcNow.Ticks.ToString();

            var request = GenerateValidCreateCheckoutRequest(orderReferenceId);
            request.MerchantCompletes = false;

            var checkoutResponse = await checkouts.CreateAsync(request);

            Assert.IsNotNull(checkoutResponse);

            Console.WriteLine("Order reference id: {0}", orderReferenceId);
            Console.WriteLine(JsonConvert.SerializeObject(checkoutResponse, Formatting.Indented));

            Assert.IsNotNull(checkoutResponse.CheckoutUrl);
            Assert.IsNotEmpty(checkoutResponse.CheckoutUrl);
            Assert.IsNotEmpty(checkoutResponse.SezzleCheckoutId.Trim());
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
            var checkouts = CreateCheckoutClient();

            var orderReferenceId = DateTime.UtcNow.Ticks.ToString();

            var request = GenerateValidCreateCheckoutRequest(orderReferenceId);
            request.MerchantCompletes = true;
            var checkoutResponse = await checkouts.CreateAsync(request);

            Assert.IsNotNull(checkoutResponse);

            Console.WriteLine("Order reference id: {0}", orderReferenceId);
            Console.WriteLine(JsonConvert.SerializeObject(checkoutResponse, Formatting.Indented));

            Assert.IsNotNull(checkoutResponse.CheckoutUrl);
            Assert.IsNotEmpty(checkoutResponse.CheckoutUrl);
            Assert.IsNotEmpty(checkoutResponse.SezzleCheckoutId.Trim());
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
        [Ignore("This is an integration test we will ignore for now until we can script it with Selenium or equivalent.")]
        public void CanCompleteACheckoutWithMerchantCompletes()
        {
            // TODO: Mark this test as ignored and require a parameter to send the order reference id into it
            var request = new CompleteCheckoutRequest()
            {
                LocalOrderId = "636773928435687781"
            };

            var checkouts = CreateCheckoutClient();

            // Now let's try completing it
            var completeResponse = checkouts.CompleteAsync(request);

            // Per Sezzle, an empty response body is success
            Assert.IsNotNull(completeResponse);
        }

        [Test]
        [Ignore("This is an integration test we will ignore for now until we can script it with Selenium or equivalent.")]
        public void CompleteCheckoutForOrderThatDoesNotExistThrowsException()
        { }

        [Test]
        [Ignore("This is an integration test we will ignore for now until we can script it with Selenium or equivalent.")]
        public void CompleteCheckoutForOrderThatExistsButHasNotBeenCompletedByTheCustomer()
        {
            //Use MerchantCompletesCheckoutFlag=true.
        }

        [Test]
        public async Task CreateCheckoutTwiceWithSameIdYieldsDifferentCheckoutLink()
        {
            var checkouts = CreateCheckoutClient();

            var orderReferenceId = DateTime.UtcNow.Ticks.ToString();

            var request01 = GenerateValidCreateCheckoutRequest(orderReferenceId);

            var response01 = await checkouts.CreateAsync(request01);

            //send exact same data twice.
            var response02 = await checkouts.CreateAsync(request01);

            Assert.IsNotNull(response01);
            Assert.IsNotNull(response02);
            Assert.False(String.IsNullOrEmpty(response01.CheckoutUrl));
            Assert.False(String.IsNullOrEmpty(response02.CheckoutUrl));

            Assert.AreNotEqual(response01.CheckoutUrl, response02.CheckoutUrl);
        }


        [Test]
        [Ignore("This is an integration test we will ignore for now until we can script it with Selenium or equivalent.")]
        public void CreatingCheckoutwithReferenceIdOfCheckoutAlreadyCompletedThrowsException()
        {
            //Use MerchantCompletesCheckoutFlag=true.
        }

        [Test]
        [Ignore("This is an integration test we will ignore for now until we can script it with Selenium or equivalent.")]
        public void CompleteCheckoutTwiceThrowsExceptionOnSecond()
        {
            //Use MerchantCompletesCheckoutFlag=true.
        }

        [Ignore("This is an integration test we will ignore for now until we can script it with Selenium or equivalent.")]
        public void CreateTwoCheckouts_ScriptCompleteTheFirst_ThrowsExceptionWhenCompleteTheSecond()
        {
            //Use MerchantCompletesCheckoutFlag=true.
        }
    }
}
