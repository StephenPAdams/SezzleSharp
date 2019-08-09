using System;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SixFourThree.SezzleSharp.Tests.Helpers.Models;

namespace SixFourThree.SezzleSharp.Tests.Helpers
{
    public class SeleniumHelper
    {
        private int countr;
        private int attempt;
        private System.Timers.Timer aTimer;
        private bool WasSuccessful;

        private IWebElement element;
        private IWebDriver driver { get; set; }
        private ClickSetting clickSetting { get; set; }
        private TypeSetting typeSetting { get; set; }

        public SeleniumHelper(ClickSetting defaultClickSetting, TypeSetting defaultTypeSetting)
        {
            countr = 0;
            clickSetting = defaultClickSetting;
            typeSetting = defaultTypeSetting;
        }
        private IWebDriver OpenBrowser(string BrowserType, string Headless = "")
        {

            if (BrowserType.ToLower() == "chrome")
            {
                OpenQA.Selenium.Chrome.ChromeOptions options = new OpenQA.Selenium.Chrome.ChromeOptions();

                options.AddArgument("--start-maximized");
                options.AddArgument("--ignore-certificate-errors");
                options.AddArgument("--disable-popup-blocking");
                options.AddArgument("--safebrowsing-disable-download-protection");
                options.AddArgument("--incognito");

                if (Headless.ToLower() == "yes" || Headless == "")
                {
                    options.AddArgument("--headless");
                }
                options.AddUserProfilePreference("download.prompt_for_download", false);
                options.AddUserProfilePreference("disable-popup-blocking", true);
                options.AddUserProfilePreference("safebrowsing", "enabled");

                IWebDriver driv = new OpenQA.Selenium.Chrome.ChromeDriver(options);
                return driv;
            }

            else return new OpenQA.Selenium.Firefox.FirefoxDriver();
        }

        public bool DidPageLoad(By THeElement, int timeout = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                wait.Until(driver => driver.FindElement(THeElement));
                return true;
            }
            catch
            {
                Log(string.Format("There was an issue finding the Page Loaded Element"));
                return false;
            }
        }
        public void Click(By ByString)
        {
            Click(ByString, this.clickSetting);
        }
        public void Click(By ByString, ClickSetting click)
        {
            var wait = new WebDriverWait(driver, click.PageTimeout);
            wait.Until(driver => driver.FindElement(ByString));

            IWebElement item = driver.FindElement(ByString);
            Click(item, click);
        }
        public void Click(IWebElement item)
        {
            countr = 0;
            bool didItWork = false;
            element = item;
            aTimer = new System.Timers.Timer();
            aTimer.Interval = clickSetting.PollInterval.TotalMilliseconds;
            aTimer.Elapsed += TimedClickOnWebElement;
            aTimer.Enabled = true;
            while (aTimer.Enabled == true)
            { }
                if (countr > clickSetting.MaxPollCount && WasSuccessful == false)
                {
                    didItWork = false;
                }
                else if (countr <= clickSetting.MaxPollCount || WasSuccessful == true)
                {
                    didItWork = true;
                }
            
            aTimer.Dispose();
            didItWork.Should().BeTrue();
            countr = 0;
        }
        public void Click(IWebElement item, ClickSetting click)
        {
            countr = 0;
            bool didItWork = false;
            element = item;
            aTimer = new System.Timers.Timer();
            aTimer.Interval = click.PollInterval.TotalMilliseconds;
            aTimer.Elapsed += TimedClickOnWebElement;
            aTimer.Enabled = true;
            while (aTimer.Enabled == true)
            {
                if (countr > click.MaxPollCount && WasSuccessful == false)
                {
                    didItWork = false;
                }
                else if (countr <= click.MaxPollCount || WasSuccessful == true)
                {
                    didItWork = true;
                }
            }
            aTimer.Dispose();
            didItWork.Should().BeTrue();
            countr = 0;
        }

        public string GetCurrentAddress()
        {
            try
            {
                Console.WriteLine(string.Format("The current URL is '{0}'", driver.Url));
                return driver.Url;
            }
            catch
            {
                Console.WriteLine("There was an issue getting the browser's address.");
                return "Current Address not available";
            }
        }
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
        
        public void Quit()
        {
            driver.Quit();
        }
        public void Start(string URL)
        {
            driver = OpenBrowser("Chrome", "Yes");
            driver.Navigate().GoToUrl(URL);
        }
        private void TimedClickOnWebElement(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (countr <= clickSetting.MaxPollCount)
            {
                try
                {
                    element.Click();
                    WasSuccessful = true;
                    aTimer.Enabled = false;
                }
                catch
                {
                    countr++;
                    Console.WriteLine(string.Format("Can't click {0} on attempt number {1}", Convert.ToString(element), Convert.ToString(countr)));
                }
            }
            else if (countr > clickSetting.MaxPollCount)
            {
                WasSuccessful = false;
                aTimer.Enabled = false;
            }
        }
        public void Type(By txtBox, string whattotype, int OneToClearTheOriginalValue = 0)
        {
            var wait = new WebDriverWait(driver, typeSetting.PageTimeout);
            wait.Until(driver => driver.FindElement(txtBox));
            wait.Until(driver => driver.FindElement(txtBox).Enabled == true);
            if (OneToClearTheOriginalValue == 1)
            { driver.FindElement(txtBox).Clear(); }
            if (whattotype.Length >= 1)
            {
                driver.FindElement(txtBox).SendKeys(whattotype);
            }
        }
    }
}
