using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using SixFourThree.SezzleSharp.Endpoints;
using SixFourThree.SezzleSharp.Models;
using SixFourThree.SezzleSharp.Models.Checkouts;
using SixFourThree.SezzleSharp.Models.Common;
using SixFourThree.SezzleSharp.Models.Orders;

namespace SixFourThree.SezzleSharp.Tests.Endpoints
{
    [TestFixture]
    public class OrdersTests : TestBase
    {
        private async Task<CreateCheckoutResponse> CreateOrder(string orderReferenceId, bool requireShippingInfo=true)
        {
            var checkouts = CreateCheckoutClient();

            var request = GenerateValidCreateCheckoutRequest(orderReferenceId, requireShippingInfo);
            request.MerchantCompletes = false;

            var checkoutResponse = await checkouts.CreateAsync(request);
            return checkoutResponse;
        }

        /// <summary>
        /// This test should have an order reference id specified to it and will check to see if it can get order details without shipping address
        /// </summary>
        /// <returns></returns>
        //[Ignore("Ignoring test until order endpoint is fixed in sandbox.")]
        [Test]
        public async Task CanGetOrderDetailsWithShipping()
        {
            // TODO: Ignore and find a way to pipe in a valid order reference id

            //arrange
            var orderReferenceId = DateTime.UtcNow.Ticks.ToString();
            var order = await CreateOrder(orderReferenceId);

            var orders = new OrdersEndpoint(_baseConfiguration, _authenticationClient, GetDefaultSezzleHttpClient());

            var request = new OrderDetailsRequest()
            {
                IncludeShippingInfo = true,
                OrderReferenceId = orderReferenceId
            };

            //act
            var orderDetails = await orders.GetDetailsAsync(request);
            Console.WriteLine(JsonConvert.SerializeObject(orderDetails, Formatting.Indented));

            //assert
            Assert.IsNotNull(orderDetails);
            Assert.IsNotNull(orderDetails.Customer);

            //this is all we care about right now is that these details that we passed in are coming back.
            //note to self, these get "reset" when someone logs into the sezzle and hits this checkout URL
            Assert.AreEqual(_testCustomerDetails.FirstName, orderDetails.Customer.FirstName);
            Assert.AreEqual(_testCustomerDetails.LastName, orderDetails.Customer.LastName);
            Assert.AreEqual(_testCustomerDetails.Email, orderDetails.Customer.Email);
            Assert.AreEqual(_testCustomerDetails.Phone, orderDetails.Customer.Phone);

            //also verify that certain fields are set correctly.
            Assert.NotNull(orderDetails);

            Assert.AreEqual(Enums.OrderStatusEnum.S0010OrderCreated, orderDetails.OrderStatus);

            //verify this enum gets deserialized correctly
            Assert.AreEqual(Enums.CheckoutCurrencyCodeEnum.USD, orderDetails.CurrencyCode);

            //customer detail does not coming back in either postman nor our library.
            //todo: shipping address object comes back, but is currently blank.
            //Assert.IsNotNull(orderDetails.ShippingAddress);
            //this is still broken.
            //Assert.IsNotEmpty(orderDetails.ShippingAddress.Name);
        }

        [Test]
        public void VerifyOrderStatusEnum_S0000UnknownOrInconsistent_Min()
        {
            var status = OrderDetailsResponse.GetOrderStatus(DateTime.MinValue, null, null);

            Assert.AreEqual(Enums.OrderStatusEnum.S0000UnknownOrInconsistent, status);
        }


        [Test]
        public void VerifyOrderStatusEnum_S0000UnknownOrInconsistent_Max()
        {
            var status = OrderDetailsResponse.GetOrderStatus(DateTime.MaxValue, null, null);

            Assert.AreEqual(Enums.OrderStatusEnum.S0000UnknownOrInconsistent, status);
        }

        [Test]
        public void VerifyOrderStatusEnum_S0010OrderCreated()
        {
            var status = OrderDetailsResponse.GetOrderStatus(DateTime.Now, null, null);

            Assert.AreEqual(Enums.OrderStatusEnum.S0010OrderCreated, status);
        }

        [Test]
        public void VerifyOrderStatusEnum_S0020UserHasCheckedOut()
        {
            var status = OrderDetailsResponse.GetOrderStatus(DateTime.Now.AddMinutes(-1), DateTime.Now, null);

            Assert.AreEqual(Enums.OrderStatusEnum.S0020UserHasCheckedOut, status);
        }

        [Test]
        public void VerifyOrderStatusEnum_S0030FundsCapturedAllDoneBoomashakalakaYippyKiYay_ForMerchantCompletesEqualsTrue()
        {
            var status = OrderDetailsResponse.GetOrderStatus(DateTime.Now.AddMinutes(-1), DateTime.Now, DateTime.Now.AddMinutes(1));

            Assert.AreEqual(Enums.OrderStatusEnum.S0030FundsCapturedAllDoneBoomshakalakaYippiekiyay, status);
        }

        [Test]
        public void VerifyOrderStatusEnum_S0030FundsCapturedAllDoneBoomashakalakaYippyKiYay_ForMerchantCompletesEqualsFalse()
        {
            var status = OrderDetailsResponse.GetOrderStatus(DateTime.Now.AddMinutes(-1), null, DateTime.Now.AddMinutes(1));

            Assert.AreEqual(Enums.OrderStatusEnum.S0030FundsCapturedAllDoneBoomshakalakaYippiekiyay, status);
        }



        [Test]
        [Ignore("Ignored because we are missing item #30 taht enables us to complete a checkout (selenium).")]
        public async Task CanCreateOrder_CustomerCompletes_MerchantCompletes_RefundPartial()
        {
            //arrange
            var orderReferenceId = DateTime.UtcNow.Ticks.ToString();

            var checkoutEndpoint = new CheckoutEndpoint(_baseConfiguration,_authenticationClient,GetDefaultSezzleHttpClient());
            var createCheckoutRequest = GenerateValidCreateCheckoutRequest(orderReferenceId);
            createCheckoutRequest.MerchantCompletes = true;
            //makes it easier to script completion of checkout by a user.
            createCheckoutRequest.RequiresShippingInfo = false;

            var completeCheckoutRequest = new CompleteCheckoutRequest(){LocalOrderId = createCheckoutRequest.OrderReferenceId};

            var ordersEndpoint = new OrdersEndpoint(_baseConfiguration, _authenticationClient, GetDefaultSezzleHttpClient());

            var request = new OrderDetailsRequest()
            {
                IncludeShippingInfo = true,
                OrderReferenceId = orderReferenceId
            };

            //create refund request
            var refundAmount = 123;
            var partialRefundRequest = new OrderRefundRequest()
            {
                Amount = new Price()
                {
                    AmountInCents = refundAmount,
                    Currency = Enums.CheckoutCurrencyCodeEnum.USD
                },
                IsFullRefund = false,
                OrderReferenceId = orderReferenceId,
                RefundId = $"gh_refund_{DateTime.UtcNow.Ticks.ToString()}",
                RefundReason = "some reason here"
            };

            //act
            //10 create order
            var order = await checkoutEndpoint.CreateAsync(createCheckoutRequest);
            Console.WriteLine(JsonConvert.SerializeObject(order, Formatting.Indented));
            
            //20 get order details
            var orderDetails = await ordersEndpoint.GetDetailsAsync(request);
            Console.WriteLine(JsonConvert.SerializeObject(orderDetails, Formatting.Indented));

            //30 (user completes checkout - use selenium) complete the checkout
            //todo: use selenium to complete the checkout through user workflow.

            //40 merchant completes the checkout.
            var completedCheckout= await checkoutEndpoint.CompleteAsync(completeCheckoutRequest);
            
            //partial refund
            var refundResponse = await ordersEndpoint.RefundAsync(partialRefundRequest);

            //test #1, verify order refund is processed and returns data as sent in.
            Assert.AreEqual(refundAmount, refundResponse.Amount.AmountInCents);
            Assert.AreEqual(partialRefundRequest.RefundId, refundResponse.RefundId);

            //get order details again.  the order value should have changed by the amount refunded.
            var orderDetails2 = await ordersEndpoint.GetDetailsAsync(request);

            //test #2, verify order amount has changed after refund
            //todo: this ammount does not actually change like I expected it to.  Perhaps there is a missing endpoint where we can see what modified the order (refunds).
            //long expectedOrderAmountAfterRefund = orderDetails.AmountInCents - refundAmount;
            //Assert.AreEqual(expectedOrderAmountAfterRefund, orderDetails2.AmountInCents);
        }




        ///// <summary>
        ///// This test should have an order reference id specified to it and will check to see if it can get order details with a shipping address
        ///// </summary>
        ///// <returns></returns>
        //[Test]
        //public void CanGetOrderDetailsWithShipping()
        //{
        //    // TODO: Ignore and find a way to pipe in a valid order reference id
        //    var orderReferenceId = "636773940180888141";
        //    var orders = new OrdersEndpoint(SezzleConfig, AuthenticationResponse);

        //    var orderDetails = orders.Get(orderReferenceId, true);
        //    Console.WriteLine(JsonConvert.SerializeObject(orderDetails, Formatting.Indented));

        //    Assert.IsNotNull(orderDetails);
        //    Assert.IsNotNull(orderDetails.Customer);
        //    Assert.IsNotNull(orderDetails.ShippingAddress);
        //}

        [Ignore("We need more order tests, once sezzle has been able to fix from their end.  Order tests are really integration tests.")]
        [Test]
        public void CanGetOrderDetails_NeedMoreTests()
        {
        }

        ///// <summary>
        ///// This test should have an order reference id specified to it and will attempt to refund the order fully
        ///// </summary>
        ///// <returns></returns>
        //[Test]
        //public void CanRefundOrderFully()
        //{
        //    // TODO: Ignore and find a way to pipe in a valid order reference id
        //    var orderReferenceId = "636773935603630168";
        //    var orders = new OrdersEndpoint(SezzleConfig, AuthenticationResponse);
        //    var refundReason = "Customer returned item.";
        //    var refundId = Guid.NewGuid().ToString();

        //    var orderRefund = new OrderRefund() { IsFullRefund = true, RefundId = refundId, RefundReason = refundReason };
        //    var orderRefundResponse = orders.RefundAsync(orderReferenceId, orderRefund);

        //    Assert.IsNotNull(orderRefundResponse);
        //    Console.WriteLine(JsonConvert.SerializeObject(orderRefundResponse, Formatting.Indented));

        //    Assert.AreEqual(orderRefundResponse.HttpStatusCode, HttpStatusCode.OK);
        //    Assert.AreEqual(orderRefundResponse.RefundId, refundId);
        //    Assert.AreEqual(orderRefundResponse.RefundReason, refundReason);

        //    // TODO: Pull order details and compare that total with the total refunded amount returned from refund API call
        //}

        ///// <summary>
        ///// This test should have an order reference id specified to it and will attempt to refund the order partially
        ///// </summary>
        ///// <returns></returns>
        //[Test]
        //public void CanRefundOrderPartially()
        //{
        //    // TODO: Ignore and find a way to pipe in a valid order reference id
        //    var orderReferenceId = "636773928435687781";
        //    var orders = new OrdersEndpoint(SezzleConfig, AuthenticationResponse);
        //    var price = new Price(500, "USD");
        //    var refundReason = "Customer returned item damaged.";
        //    var refundId = Guid.NewGuid().ToString();

        //    var orderRefund = new OrderRefund() { Amount = price, RefundId = refundId, RefundReason = refundReason };
        //    var orderRefundResponse = orders.RefundAsync(orderReferenceId, orderRefund);

        //    Assert.IsNotNull(orderRefundResponse);
        //    Console.WriteLine(JsonConvert.SerializeObject(orderRefundResponse, Formatting.Indented));

        //    Assert.AreEqual(orderRefundResponse.HttpStatusCode, HttpStatusCode.OK);
        //    Assert.AreEqual(orderRefundResponse.RefundId, refundId);
        //    Assert.AreEqual(orderRefundResponse.RefundReason, refundReason);
        //    Assert.AreEqual(orderRefundResponse.Amount.AmountInCents, price.AmountInCents);
        //}
    }
}
