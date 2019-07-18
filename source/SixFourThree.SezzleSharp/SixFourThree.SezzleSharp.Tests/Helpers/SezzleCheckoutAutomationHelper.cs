using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using FluentAssertions;
using SixFourThree.SezzleSharp.Tests.Models;
using SixFourThree.SezzleSharp.Tests.Endpoints;
using SixFourThree.SezzleSharp.Tests.Helpers.Models;

namespace SixFourThree.SezzleSharp.Tests.Helpers
{
    public class SezzleCheckoutAutomationHelper : TestBase
    {
        private ClickSetting _defaultClickSetting = null;
        private TypeSetting _defaultTypeSetting = null;

        public SezzleCheckoutAutomationHelper(ClickSetting defaultClickSetting,TypeSetting defaultTypeSetting )
        {
            _defaultClickSetting = defaultClickSetting;
            _defaultTypeSetting = defaultTypeSetting;
        }

        //use selenium to complete the checkout
        public void CompleteCheckoutClientSide(string checkoutUrl, CustomerLoginCredentials customerLoginCredentials, string CheckOutCompleteAddress)
        {
            SeleniumHelper seleniumHelper = new SeleniumHelper(_defaultClickSetting, _defaultTypeSetting);

            try
            {
                //  Start chrome browser and navigate to supplied URL
                seleniumHelper.Start(checkoutUrl);

                //Login
                seleniumHelper.Click(btn_Welcome_Login());
                seleniumHelper.Type(tb_Login_PhoneNumber(), customerLoginCredentials.PhoneNumber);

                seleniumHelper.Type(tb_Login_Pin(), customerLoginCredentials.Pin);
                seleniumHelper.Click(btn_Login_Login());

                //Enter Otp Code and continue

                seleniumHelper.Type(tb_Verify_Pin(), customerLoginCredentials.OtpCode);
                //sezzled continue click button is weird, so we manually inject a value here higher than we normally use for polling time.
                seleniumHelper.Click(btn_Verify_Continue(), new ClickSetting { PageTimeout = TimeSpan.FromSeconds(10), PollInterval = TimeSpan.FromMilliseconds(500) });

                //Complete the Order
                seleniumHelper.Click(btn_Autopay_CompleteOrder());

                //Waits for the Checkout Complete page to load
                seleniumHelper.DidPageLoad(tb_Google_Q()).Should().BeTrue();
                //Verify redirect to complete page
                Assert.AreEqual(CheckOutCompleteAddress, seleniumHelper.GetCurrentAddress());

                //Dump the browser, Log the completion and return true for "It worked"
                seleniumHelper.Quit();
                Console.WriteLine("There the order is now complete from the client's sezzle experience.");
            }
            catch
            {
                //Dump the browser, Log the failure of the process and return false for "Its broken"
                seleniumHelper.Quit();
                throw new Exception("There was an issue completing the order.");
            }
        }

        private static By btn_Autopay_CompleteOrder()
        {
            return By.XPath("//button[@class='fullwidth btn-green btn-lg']//span[contains(.,'Complete Order')]");
        }
        private static By tb_Google_Q()
        {
            return By.Name("q");
        }
        private static By btn_Login_Login()
        {
            return By.XPath("//button[@class='fullwidth btn-green btn-lg' and contains(.,'Login')]");
        }

        private static By btn_Welcome_Login()
        {
            return By.XPath("//button[@class='fullwidth button btn-green btn-lg margin-top--x2']//span[contains(.,'Log in')]");
        }
        private static By btn_Verify_Continue()
        {
            return By.XPath("//button[@class='fullwidth btn-green btn-lg' and contains(.,'Continue')]");
        }

        private static By tb_Login_PhoneNumber()
        {
            return By.XPath("//input[@placeholder = 'Mobile Phone']");
        }

        private static By tb_Login_Pin()
        {
            return By.XPath("//input[@type = 'tel' and @name = 'pin']");
        }

        private static By tb_Verify_Pin()
        {
            return By.XPath("//div[@class='sezzle-number-box-wrapper sezzle-number-box-focused']//input[@class='sezzle-hidden-number-input']");
        }
    }
}
