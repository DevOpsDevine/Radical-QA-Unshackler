using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium.Remote;
using log4net;

namespace Radical.Tests
{
    public class GoogleTests:SpecificationForAllBrowsers
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(WebDriverFactoryTest));

        public override void SpecifyForBrowser(RemoteWebDriver driver)
        {
            given("a non null driver", delegate()
            {
                Assert.IsNotNull(driver);
                when("we goto google.com", delegate()
                {                                                                                   
                    driver.Navigate().GoToUrl("http://google.com");
                    then("we can get the user agent",
                        delegate()
                            {
                                expect(() => GetUserAgent(driver).Length >0);
                            });
                });

            });
        }

        private string GetUserAgent(RemoteWebDriver driver)
        {
            return driver.ExecuteScript("return window.navigator.userAgent;").ToString();
        }
    }
}
