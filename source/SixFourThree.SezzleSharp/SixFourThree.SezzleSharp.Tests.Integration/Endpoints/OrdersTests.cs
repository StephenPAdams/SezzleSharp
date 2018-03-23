using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using SixFourThree.SezzleSharp.Endpoints;
using SixFourThree.SezzleSharp.Models;

namespace SixFourThree.SezzleSharp.Tests.Integration.Endpoints
{
    [TestFixture]
    public class OrdersTests : BaseTests
    {
        /// <summary>
        /// This test should have an order reference id specified to it and will check to see if it can get order details without shipping address
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CanGetOrderDetailsWithoutShipping()
        {
            // TODO: Ignore and find a way to pipe in a valid order reference id
            var orderReferenceId = "636573728070321239";
            var orders = new Orders(SezzleConfig, AuthResponse);

            var orderDetails = await orders.Get(orderReferenceId, false);
            Console.WriteLine(JsonConvert.SerializeObject(orderDetails, Formatting.Indented));

            Assert.IsNotNull(orderDetails);
            Assert.IsNull(orderDetails.ShippingAddress);
        }

        /// <summary>
        /// This test should have an order reference id specified to it and will check to see if it can get order details with a shipping address
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CanGetOrderDetailsWithShipping()
        {
            // TODO: Ignore and find a way to pipe in a valid order reference id
            var orderReferenceId = "636573728070321239";
            var orders = new Orders(SezzleConfig, AuthResponse);

            var orderDetails = await orders.Get(orderReferenceId, true);
            Console.WriteLine(JsonConvert.SerializeObject(orderDetails, Formatting.Indented));

            Assert.IsNotNull(orderDetails);
            Assert.IsNotNull(orderDetails.ShippingAddress);
        }

        /// <summary>
        /// This test should have an order reference id specified to it and will attempt to refund the order fully
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CanRefundOrderFully()
        {
            // TODO: Ignore and find a way to pipe in a valid order reference id
            var orderReferenceId = "636573728070321239";
            var orders = new Orders(SezzleConfig, AuthResponse);
            var refundReason = "Customer returned item.";
            var refundId = Guid.NewGuid().ToString();

            var orderRefund = new OrderRefund() { IsFullRefund = true, RefundId = refundId, RefundReason = refundReason };
            var orderRefundResponse = await orders.Refund(orderReferenceId, orderRefund);

            Assert.IsNotNull(orderRefundResponse);
            Console.WriteLine(JsonConvert.SerializeObject(orderRefundResponse, Formatting.Indented));

            Assert.AreEqual(orderRefundResponse.HttpStatusCode, HttpStatusCode.OK);
            Assert.AreEqual(orderRefundResponse.RefundId, refundId);
            Assert.AreEqual(orderRefundResponse.RefundReason, refundReason);
            
            // TODO: Pull order details and compare that total with the total refunded amount returned from refund API call
        }

        /// <summary>
        /// This test should have an order reference id specified to it and will attempt to refund the order partially
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CanRefundOrderPartially()
        {
            // TODO: Ignore and find a way to pipe in a valid order reference id
            var orderReferenceId = "636573728070321239";
            var orders = new Orders(SezzleConfig, AuthResponse);
            var price = new Price(500, "USD");
            var refundReason = "Customer returned item damaged.";
            var refundId = Guid.NewGuid().ToString();

            var orderRefund = new OrderRefund() { Amount = price, RefundId = refundId, RefundReason = refundReason };
            var orderRefundResponse = await orders.Refund(orderReferenceId, orderRefund);

            Assert.IsNotNull(orderRefundResponse);
            Console.WriteLine(JsonConvert.SerializeObject(orderRefundResponse, Formatting.Indented));

            Assert.AreEqual(orderRefundResponse.HttpStatusCode, HttpStatusCode.OK);
            Assert.AreEqual(orderRefundResponse.RefundId, refundId);
            Assert.AreEqual(orderRefundResponse.RefundReason, refundReason);
            Assert.AreEqual(orderRefundResponse.Amount.AmountInCents, price.AmountInCents);
        }
    }
}
