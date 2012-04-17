using NJasmine;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Radical.Infrastructure;
using log4net;

namespace Radical.Tests
{
    public abstract class SpecificationForAllBrowsers : GivenWhenThenFixture
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(WebDriverFactoryTest));
        string[] SupportedBrowsers = new string[] { BrowserNames.Chrome, BrowserNames.Firefox, BrowserNames.InternetExplorer };


        public abstract void SpecifyForBrowser(RemoteWebDriver browser);

        public sealed override void Specify()
        {
            foreach(string browserName in SupportedBrowsers)
            {
                given(browserName, delegate
                {
                    SpecifyForBrowser(CreateWebDriver(browserName));  
                });
            }
        }

        private static WebStartupSettings LoadSettings()
        {
            return new WebStartupSettings();
        }

        private RemoteWebDriver CreateWebDriver(string browserType)
        {
            var settings = LoadSettings();
            settings.RunTestsUsingSeleniumGrid = false;
            settings.BrowserType = browserType;

            return new WebDriverFactory(settings).CreateWebDriver();
            
        }

    }
}
