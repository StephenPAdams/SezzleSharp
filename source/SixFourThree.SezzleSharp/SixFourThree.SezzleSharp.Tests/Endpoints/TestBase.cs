using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using SixFourThree.SezzleSharp.Configuration;
using SixFourThree.SezzleSharp.Endpoints;
using SixFourThree.SezzleSharp.Extensions;
using SixFourThree.SezzleSharp.Interfaces;
using SixFourThree.SezzleSharp.Models;
using SixFourThree.SezzleSharp.Models.Authentication;
using SixFourThree.SezzleSharp.Models.Checkouts;
using SixFourThree.SezzleSharp.Models.Checkouts.Supporting;
using SixFourThree.SezzleSharp.Models.Common;
using SixFourThree.SezzleSharp.Tests.Models;

namespace SixFourThree.SezzleSharp.Tests.Endpoints
{
    public abstract class TestBase
    {
        protected BaseConfiguration _baseConfiguration;
        protected AuthenticationConfiguration _authenticationConfiguration;

        protected IAuthenticationEndpoint _authenticationClient;

        protected CustomerDetails _testCustomerDetails = new CustomerDetails("TestFirst", "TestLast", "test@sezzle.com"){Phone="123-123-1234"};

        //it would not read the app.config, so I found this here.  https://stackoverflow.com/questions/48666780/cant-read-app-config-in-c-sharp-net-core-unit-test-project-with-configurationm
        private KeyValueConfigurationCollection LocalAppSettings => ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings;

        private AuthenticationConfiguration GetAuthenticationConfiguration()
        {
            return new AuthenticationConfiguration()
            {

                ApiPublicKey = LocalAppSettings["Sezzle.Authentication.ApiPublicKey"].Value,
                ApiPrivateKey = LocalAppSettings["Sezzle.Authentication.ApiPrivateKey"].Value,
            };
        }
        private BaseConfiguration GetBaseConfiguration()
        {
            return new BaseConfiguration()
            {
                ApiUrl = LocalAppSettings["Sezzle.Base.ApiUrl"].Value
            };
        }

        private SezzleHttpClientConfiguration GetSezzleHttpClientConfiguration()
        {
            return new SezzleHttpClientConfiguration()
            {
                Timeout = TimeSpan.Parse(LocalAppSettings["Sezzle.HttpClient.Timeout"].Value)
            };
        }

        public CustomerLoginCredentials GetCustomerLoginCredentials()
        {
            return new CustomerLoginCredentials()
            {
                PhoneNumber = LocalAppSettings["Sezzle.CustomerLoginCredentials.PhoneNumber"].Value,
                Pin = LocalAppSettings["Sezzle.CustomerLoginCredentials.Pin"].Value,
                OtpCode = LocalAppSettings["Sezzle.CustomerLoginCredentials.OtpCode"].Value
            };

        }

        protected ISezzleHttpClient GetDefaultSezzleHttpClient()
        {
            return new SezzleHttpClient(GetSezzleHttpClientConfiguration());
        }

        protected CheckoutEndpoint CreateCheckoutClient()
        {
            return new CheckoutEndpoint(_baseConfiguration, _authenticationClient, GetDefaultSezzleHttpClient());
        }

        [SetUp]
        public async Task Setup()
        {
            _baseConfiguration = GetBaseConfiguration();
            _authenticationConfiguration = GetAuthenticationConfiguration();

            _authenticationClient = new AuthenticationEndpoint(_baseConfiguration, _authenticationConfiguration, GetDefaultSezzleHttpClient());
        }

        public CreateCheckoutRequest GenerateValidCreateCheckoutRequest(string ourOrderReferenceId, bool requiresShippingInfo=true)
        {
            var checkoutRequest = new CreateCheckoutRequest();
            //$12.34
            var amount = 1234;
            var currencyCode = Enums.CheckoutCurrencyCodeEnum.USD;

            checkoutRequest.AmountInCents = amount;
            checkoutRequest.CurrencyCode = currencyCode;
            checkoutRequest.OrderDescription = "Test checkout";
            checkoutRequest.OrderReferenceId = ourOrderReferenceId;
            checkoutRequest.CheckoutCancelUrl = "https://test.sezzle.com/cancel";
            //Changing the CheckoutCompleteUrl for lower environments. This will navigate to a real page to allow intigration tests to pick up an element at the end of the checkout.
            checkoutRequest.CheckoutCompleteUrl = "https://www.google.com/";
            checkoutRequest.CustomerDetails = _testCustomerDetails;
            checkoutRequest.BillingAddress = new Address("TestFirst TestLast", "1516 W. Lake St", "Minneapolis", "MN", "55408", "US");
            checkoutRequest.ShippingAddress = new Address("TestFirst TestLast", "1516 W. Lake St", "Minneapolis", "MN", "55408", "US");
            checkoutRequest.Items = new List<Item> { new Item("Test T-Shirt", "ABC123", 1, new Price(){AmountInCents = amount,Currency = currencyCode }) };
            //sezzle appears to not store the shipping information without this flag being set.
            checkoutRequest.RequiresShippingInfo = requiresShippingInfo;

            return checkoutRequest;
        }
    }
}
