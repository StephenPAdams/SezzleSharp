using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using SixFourThree.SezzleSharp.Endpoints;
using SixFourThree.SezzleSharp.Exceptions.Specific;
using SixFourThree.SezzleSharp.Models;
using SixFourThree.SezzleSharp.Models.Checkouts;
using SixFourThree.SezzleSharp.Models.Health;
using SixFourThree.SezzleSharp.Models.Orders;
using SixFourThree.SezzleSharp.Tests.Endpoints;
using SixFourThree.SezzleSharp.Tests.Models;
using OpenQA.Selenium;
using SixFourThree.SezzleSharp.Models.Common;
using SixFourThree.SezzleSharp.Tests.Helpers;
using SixFourThree.SezzleSharp.Tests.Helpers.Models;


namespace SixFourThree.SezzleSharp.Tests
{
    [TestFixture]
    public class IntegrationTests : TestBase
    {
        private SezzleCheckoutAutomationHelper _sezzleCheckoutAutomationHelper;
        
        [SetUp]
        public void Init()
        {
            var sezzleTimeout = new TimeSpan(0, 0, 30);
            var sezzlePagePollInterval = new TimeSpan(0,0,0,0,50);

            _sezzleCheckoutAutomationHelper = new SezzleCheckoutAutomationHelper(
                new ClickSetting() { PageTimeout = sezzleTimeout, PollInterval = sezzlePagePollInterval },
                new TypeSetting() { PageTimeout = sezzleTimeout, PollInterval = sezzlePagePollInterval }
                );
        }

        [Test]
        public async Task FullWorkflowWithMerchantCompletesEqualsTrue_Positive()
        {
            //** Create the Checkout/Order
            var checkouts = CreateCheckoutClient();
            var orderReferenceId = DateTime.UtcNow.Ticks.ToString();
            var createCheckoutRequest = GenerateValidCreateCheckoutRequest(orderReferenceId, false);
            createCheckoutRequest.MerchantCompletes = true;
            var checkoutResponse = await checkouts.CreateAsync(createCheckoutRequest);
            Console.WriteLine(checkoutResponse.CheckoutUrl);

            // ** get order details.  
            //verify that order is in customer NotPaid status.
            var orders = new OrdersEndpoint(_baseConfiguration, _authenticationClient, GetDefaultSezzleHttpClient());
            var orderDetailsRequest = new OrderDetailsRequest()
            {
                IncludeShippingInfo = true,
                OrderReferenceId = orderReferenceId
            };

            // ** check order status. get order details before user has been redirected to the checkout URL
            var orderDetailsBeforeUserChecksOut = await orders.GetDetailsAsync(orderDetailsRequest);
            Assert.AreEqual(orderDetailsBeforeUserChecksOut.OrderStatus, Enums.OrderStatusEnum.S0010OrderCreated);

            //***********Existing Customer goes to checkout And completes checkout        
            _sezzleCheckoutAutomationHelper.CompleteCheckoutClientSide(checkoutResponse.CheckoutUrl, base.GetCustomerLoginCredentials(), createCheckoutRequest.CheckoutCompleteUrl);

            // verify order status is now in the checked out status
            var orderDetailsBeforeMerchantCompletesCheckout = await orders.GetDetailsAsync(orderDetailsRequest);
            Assert.AreEqual(orderDetailsBeforeMerchantCompletesCheckout.OrderStatus, Enums.OrderStatusEnum.S0020UserHasCheckedOut);

            //merchant completes the checkout/order. (we had used merchantcompletes=true)
            var completeCheckoutRequest = new CompleteCheckoutRequest()
            {
                LocalOrderId = orderReferenceId
            };
            await checkouts.CompleteAsync(completeCheckoutRequest);

            //get order details.  order should now be complete, meaning sezzle has captured the funds and we are guaranteed the full amount.
            var orderDetailsAfterMerchantCompletesCheckout = await orders.GetDetailsAsync(orderDetailsRequest);
            Assert.AreEqual(Enums.OrderStatusEnum.S0030FundsCapturedAllDoneBoomshakalakaYippiekiyay, orderDetailsAfterMerchantCompletesCheckout.OrderStatus);
        }

        /// <summary>
        /// Create, customer completes, sezzle completes automatically, and get order details confirms all of this has happened.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task FullCheckoutWorkflowWithMerchantCompletesEqualsFalse_Positive()
        {
            //** Create the Checkout/Order
            var checkouts = CreateCheckoutClient();
            var orderReferenceId = DateTime.UtcNow.Ticks.ToString();
            var createCheckoutRequest = GenerateValidCreateCheckoutRequest(orderReferenceId, false);
            createCheckoutRequest.MerchantCompletes = false;
            var checkoutResponse = await checkouts.CreateAsync(createCheckoutRequest);

            // ** get order details.  
            //verify that order is in customer NotPaid status.
            var orders = new OrdersEndpoint(_baseConfiguration, _authenticationClient, GetDefaultSezzleHttpClient());
            var orderDetailsRequest = new OrderDetailsRequest()
            {
                IncludeShippingInfo = true,
                OrderReferenceId = orderReferenceId
            };

            // ** check order status. get order details before user has been redirected to the checkout URL
            var orderDetailsBeforeUserChecksOut = await orders.GetDetailsAsync(orderDetailsRequest);
            Assert.AreEqual(orderDetailsBeforeUserChecksOut.OrderStatus, Enums.OrderStatusEnum.S0010OrderCreated);

            //***********Existing Customer goes to checkout And completes checkout
            _sezzleCheckoutAutomationHelper.CompleteCheckoutClientSide(checkoutResponse.CheckoutUrl, base.GetCustomerLoginCredentials(), createCheckoutRequest.CheckoutCompleteUrl);

            Console.WriteLine($"OrderId = '{orderReferenceId}'");

            // verify order status - since we passed "merchantcompletes=false" earlier, the order is completed as soon as the user clicks 'submit'_checkoutLoginPhoneNumber
            //get order details.  order should now be complete, meaning sezzle has captured the funds and we are guaranteed the full amount.
            var orderDetailsAfterMerchantCompletesCheckout = await orders.GetDetailsAsync(orderDetailsRequest);

            Assert.AreEqual(Enums.OrderStatusEnum.S0030FundsCapturedAllDoneBoomshakalakaYippiekiyay, orderDetailsAfterMerchantCompletesCheckout.OrderStatus);
        }

        /// <summary>
        /// Merchant creates a checkout then the merchant attempts to complete the checkout without the customer completing it.  
        /// This should not be possible so verify here.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task FullWorkflowWithMerchantCompletesEqualsTrue_AttemptToCompleteCheckoutWithoutCustomerCompleting_Negative()
        {
            //** Create the Checkout/Order
            var checkouts = CreateCheckoutClient();
            var orderReferenceId = DateTime.UtcNow.Ticks.ToString();
            var createCheckoutRequest = GenerateValidCreateCheckoutRequest(orderReferenceId, false);
            createCheckoutRequest.MerchantCompletes = true;
            var checkoutResponse = await checkouts.CreateAsync(createCheckoutRequest);

            // ** get order details.  
            //verify that order is in customer NotPaid status.
            var orders = new OrdersEndpoint(_baseConfiguration, _authenticationClient, GetDefaultSezzleHttpClient());
            var orderDetailsRequest = new OrderDetailsRequest()
            {
                IncludeShippingInfo = true,
                OrderReferenceId = orderReferenceId
            };

            // ** check order status. get order details before user has been redirected to the checkout URL
            var orderDetailsBeforeUserChecksOut = await orders.GetDetailsAsync(orderDetailsRequest);
            Assert.AreEqual(orderDetailsBeforeUserChecksOut.OrderStatus, Enums.OrderStatusEnum.S0010OrderCreated);

            Func<Task> act = async () =>
            {
                //merchant completes the checkout/order. (we had used merchantcompletes=true)
                var completeCheckoutRequest = new CompleteCheckoutRequest()
                {
                    LocalOrderId = orderReferenceId
                };
                var response = await checkouts.CompleteAsync(completeCheckoutRequest);
            };

            //exception is thrown from sezzle because you cannot complete a checkout that has not been completed by user.
            act.Should().Throw<SezzleErrorResponseException>();
        }

        /// <summary>
        /// creates an order, completes the order, then issues a partial refund on the order.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task FullWorkflowWithMerchantCompletesEqualsTrue_ThenRefundPartialAmount()
        {
            //** Create the Checkout/Order
            var checkouts = CreateCheckoutClient();
            var orderReferenceId = DateTime.UtcNow.Ticks.ToString();
            var createCheckoutRequest = GenerateValidCreateCheckoutRequest(orderReferenceId, false);
            createCheckoutRequest.MerchantCompletes = true;
            var checkoutResponse = await checkouts.CreateAsync(createCheckoutRequest);
            Console.WriteLine(checkoutResponse.CheckoutUrl);

            // ** get order details.  
            //verify that order is in customer NotPaid status.
            var orders = new OrdersEndpoint(_baseConfiguration, _authenticationClient, GetDefaultSezzleHttpClient());
            var orderDetailsRequest = new OrderDetailsRequest()
            {
                IncludeShippingInfo = true,
                OrderReferenceId = orderReferenceId
            };

            // ** check order status. get order details before user has been redirected to the checkout URL
            var orderDetailsBeforeUserChecksOut = await orders.GetDetailsAsync(orderDetailsRequest);
            Assert.AreEqual(orderDetailsBeforeUserChecksOut.OrderStatus, Enums.OrderStatusEnum.S0010OrderCreated);

            //***********Existing Customer goes to checkout And completes checkout
            _sezzleCheckoutAutomationHelper.CompleteCheckoutClientSide(checkoutResponse.CheckoutUrl, base.GetCustomerLoginCredentials(), createCheckoutRequest.CheckoutCompleteUrl);

            // verify order status is now in the checked out status
            var orderDetailsBeforeMerchantCompletesCheckout = await orders.GetDetailsAsync(orderDetailsRequest);
            Assert.AreEqual(orderDetailsBeforeMerchantCompletesCheckout.OrderStatus, Enums.OrderStatusEnum.S0020UserHasCheckedOut);

            //merchant completes the checkout/order. (we had used merchantcompletes=true)
            var completeCheckoutRequest = new CompleteCheckoutRequest()
            {
                LocalOrderId = orderReferenceId
            };
            await checkouts.CompleteAsync(completeCheckoutRequest);

            //get order details.  order should now be complete, meaning sezzle has captured the funds and we are guaranteed the full amount.
            var orderDetailsAfterMerchantCompletesCheckout = await orders.GetDetailsAsync(orderDetailsRequest);
            Assert.AreEqual(Enums.OrderStatusEnum.S0030FundsCapturedAllDoneBoomshakalakaYippiekiyay, orderDetailsAfterMerchantCompletesCheckout.OrderStatus);

            //now we refund

            var refundAmount = 111;

            var refundRequest = new OrderRefundRequest()
            {
                Amount = new Price()
                {
                    AmountInCents = refundAmount,
                    Currency = Enums.CheckoutCurrencyCodeEnum.USD
                },
                OrderReferenceId = orderReferenceId,
                IsFullRefund = false,
                RefundId = Guid.NewGuid().ToString(),
                RefundReason = "unit test reason"
            };

            var refundResponse = await orders.RefundAsync(refundRequest);
            //verifies amount was refunded
            Assert.AreEqual(refundAmount, refundResponse.Amount.AmountInCents);

            //get order status and verify that it is equal to the full original amount.  there are no endpoints that get the history of the refunds at this time.
            var orderStatusAfterPartialRefund = await orders.GetDetailsAsync(orderDetailsRequest);
            Assert.AreEqual(createCheckoutRequest.AmountInCents, orderStatusAfterPartialRefund.AmountInCents);
        }

        //todo: we may have to verify the error message type that we expect it to throw.
        /// <summary>
        /// Create a checkout, complete the checkout, then refund an amount greater than the the original checkout amount.  Verify an exception is thrown.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task FullWorkflowWithMerchantCompletesEqualsTrue_ThenRefundInvalidAmount_Negative()
        {
            //** Create the Checkout/Order
            var checkouts = CreateCheckoutClient();
            var orderReferenceId = DateTime.UtcNow.Ticks.ToString();
            var createCheckoutRequest = GenerateValidCreateCheckoutRequest(orderReferenceId, false);
            createCheckoutRequest.MerchantCompletes = true;
            var checkoutResponse = await checkouts.CreateAsync(createCheckoutRequest);
            Console.WriteLine(checkoutResponse.CheckoutUrl);

            // ** get order details.  
            //verify that order is in customer NotPaid status.
            var orders = new OrdersEndpoint(_baseConfiguration, _authenticationClient, GetDefaultSezzleHttpClient());
            var orderDetailsRequest = new OrderDetailsRequest()
            {
                IncludeShippingInfo = true,
                OrderReferenceId = orderReferenceId
            };

            // ** check order status. get order details before user has been redirected to the checkout URL
            var orderDetailsBeforeUserChecksOut = await orders.GetDetailsAsync(orderDetailsRequest);
            Assert.AreEqual(orderDetailsBeforeUserChecksOut.OrderStatus, Enums.OrderStatusEnum.S0010OrderCreated);

            //***********Existing Customer goes to checkout And completes checkout
            _sezzleCheckoutAutomationHelper.CompleteCheckoutClientSide(checkoutResponse.CheckoutUrl, base.GetCustomerLoginCredentials(), createCheckoutRequest.CheckoutCompleteUrl);

            // verify order status is now in the checked out status
            var orderDetailsBeforeMerchantCompletesCheckout = await orders.GetDetailsAsync(orderDetailsRequest);
            Assert.AreEqual(orderDetailsBeforeMerchantCompletesCheckout.OrderStatus, Enums.OrderStatusEnum.S0020UserHasCheckedOut);

            //merchant completes the checkout/order. (we had used merchantcompletes=true)
            var completeCheckoutRequest = new CompleteCheckoutRequest()
            {
                LocalOrderId = orderReferenceId
            };
            await checkouts.CompleteAsync(completeCheckoutRequest);

            //get order details.  order should now be complete, meaning sezzle has captured the funds and we are guaranteed the full amount.
            var orderDetailsAfterMerchantCompletesCheckout = await orders.GetDetailsAsync(orderDetailsRequest);
            Assert.AreEqual(Enums.OrderStatusEnum.S0030FundsCapturedAllDoneBoomshakalakaYippiekiyay, orderDetailsAfterMerchantCompletesCheckout.OrderStatus);

            //**********************up to now, that is just a standard order.

            // try to refund an amount greater than the initial checkout amount.  Verify our framework throws an exception.
            Func<Task> act = async () =>
            {
                //set the refund amount to 1 cent over the entire order.
                var refundAmount = createCheckoutRequest.AmountInCents + 1;

                var refundRequest = new OrderRefundRequest()
                {
                    Amount = new Price()
                    {
                        AmountInCents = refundAmount,
                        Currency = Enums.CheckoutCurrencyCodeEnum.USD
                    },
                    OrderReferenceId = orderReferenceId,
                    IsFullRefund = false,
                    RefundId = Guid.NewGuid().ToString(),
                    RefundReason = "unit test reason"
                };

                var refundResponse = await orders.RefundAsync(refundRequest);
            };

            //exception is thrown from sezzle because you cannot complete a checkout that has not been completed by user.
            act.Should().Throw<SezzleErrorResponseException>();
        }

        /// <summary>
        /// creates an order, completes the order, then issues a partial refund on the order.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task FullWorkflowWithMerchantCompletesEqualsTrue_ThenRefundFullAmount_Positive()
        {
            //** Create the Checkout/Order
            var checkouts = CreateCheckoutClient();
            var orderReferenceId = DateTime.UtcNow.Ticks.ToString();
            var createCheckoutRequest = GenerateValidCreateCheckoutRequest(orderReferenceId, false);
            createCheckoutRequest.MerchantCompletes = true;
            var checkoutResponse = await checkouts.CreateAsync(createCheckoutRequest);
            Console.WriteLine(checkoutResponse.CheckoutUrl);

            // ** get order details.  
            //verify that order is in customer NotPaid status.
            var orders = new OrdersEndpoint(_baseConfiguration, _authenticationClient, GetDefaultSezzleHttpClient());
            var orderDetailsRequest = new OrderDetailsRequest()
            {
                IncludeShippingInfo = true,
                OrderReferenceId = orderReferenceId
            };

            // ** check order status. get order details before user has been redirected to the checkout URL
            var orderDetailsBeforeUserChecksOut = await orders.GetDetailsAsync(orderDetailsRequest);
            Assert.AreEqual(orderDetailsBeforeUserChecksOut.OrderStatus, Enums.OrderStatusEnum.S0010OrderCreated);

            //***********Existing Customer goes to checkout And completes checkout
            _sezzleCheckoutAutomationHelper.CompleteCheckoutClientSide(checkoutResponse.CheckoutUrl, base.GetCustomerLoginCredentials(), createCheckoutRequest.CheckoutCompleteUrl);

            // verify order status is now in the checked out status
            var orderDetailsBeforeMerchantCompletesCheckout = await orders.GetDetailsAsync(orderDetailsRequest);
            Assert.AreEqual(orderDetailsBeforeMerchantCompletesCheckout.OrderStatus, Enums.OrderStatusEnum.S0020UserHasCheckedOut);

            //merchant completes the checkout/order. (we had used merchantcompletes=true)
            var completeCheckoutRequest = new CompleteCheckoutRequest()
            {
                LocalOrderId = orderReferenceId
            };
            await checkouts.CompleteAsync(completeCheckoutRequest);

            //get order details.  order should now be complete, meaning sezzle has captured the funds and we are guaranteed the full amount.
            var orderDetailsAfterMerchantCompletesCheckout = await orders.GetDetailsAsync(orderDetailsRequest);
            Assert.AreEqual(Enums.OrderStatusEnum.S0030FundsCapturedAllDoneBoomshakalakaYippiekiyay, orderDetailsAfterMerchantCompletesCheckout.OrderStatus);

            //**********************up to now, that is just a standard order.
            //now we refund

            var refundAmount = createCheckoutRequest.AmountInCents;

            var refundRequest = new OrderRefundRequest()
            {
                Amount = new Price()
                {
                    AmountInCents = refundAmount,
                    Currency = Enums.CheckoutCurrencyCodeEnum.USD
                },
                OrderReferenceId = orderReferenceId,
                IsFullRefund = false,
                RefundId = Guid.NewGuid().ToString(),
                RefundReason = "unit test reason"
            };

            var refundResponse = await orders.RefundAsync(refundRequest);
            //verifies amount was refunded
            Assert.AreEqual(refundAmount, refundResponse.Amount.AmountInCents);

            //get order status and verify that it is equal to the full original amount.  there are no endpoints that get the history of the refunds at this time.
            var orderStatusAfterPartialRefund = await orders.GetDetailsAsync(orderDetailsRequest);
            Assert.AreEqual(createCheckoutRequest.AmountInCents, orderStatusAfterPartialRefund.AmountInCents);
        }
    }
}
