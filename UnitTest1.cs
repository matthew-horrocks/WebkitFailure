using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;

namespace WebkitFailure
{
    public class PassingTest
    {
        public IWebDriver Driver;
        private readonly string _CBTusername = "<username>";
        private readonly string _CBTauthkey = "<password>";

        [SetUp]
        public void Setup()
        {
            var caps = new DesiredCapabilities();
            caps.SetCapability("username", _CBTusername);
            caps.SetCapability("password", _CBTauthkey);
            caps.SetCapability("browserName", "Safari");

            // uncomment for passing test
            caps.SetCapability("version", "12");
            caps.SetCapability("platform", "Mac OSX 10.14");

            // uncomment for failing test
            //caps.SetCapability("version", "13");
            //caps.SetCapability("platform", "Mac OSX 10.15");


            caps.SetCapability("screenResolution", "1920x1200");
            Driver = new RemoteWebDriver(new Uri("http://hub.crossbrowsertesting.com:80/wd/hub"), caps, TimeSpan.FromSeconds(180));
            Driver.Manage().Window.Maximize();
        }

        private IWebElement ElementClick(IWebElement element)
        {
            bool elementClickable = true;
            if (!element.Displayed) elementClickable = false;
            if (!element.Enabled) elementClickable = false;
            return elementClickable ? element : null;
        }

        public IWebElement GetElementOnceClickable(By elementLocator)
        {
            WebDriverWait wait = new WebDriverWait(Driver, new TimeSpan(0, 0, 60));

            IWebElement element = wait.Until((d) =>
            {
                try
                {
                    return ElementClick(d.FindElement(elementLocator));
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });
            return element;
        }

        [Test]
        public void PassingTest1()
        {
            Driver.Navigate().GoToUrl("https://jsfiddle.net/matthewhorrocks/w42tn053/7/show/");
            Driver.SwitchTo().Frame(0);
            GetElementOnceClickable(By.Id("show")).Click();
            GetElementOnceClickable(By.Id("showme"));
            System.Threading.Thread.Sleep(10000);
            Driver.Quit();
        }
    }
}