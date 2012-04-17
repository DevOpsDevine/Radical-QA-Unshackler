using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium.Remote;
using Radical.Infrastructure;
using log4net;

namespace Radical.Tests
{
	[TestFixture]
	public class WebDriverFactoryTest
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(WebDriverFactoryTest));

		private static WebStartupSettings LoadSettings()
		{
			return new WebStartupSettings();
		}

		[Test]
		public void CreateWebDriver_local(
			[Values(BrowserNames.Chrome, BrowserNames.Firefox)]
			string browserType)
		{
			var settings = LoadSettings();
			settings.RunTestsUsingSeleniumGrid = false;
			settings.BrowserType = browserType; 
				
			var driver = new WebDriverFactory(settings).CreateWebDriver();
			CheckDriverThenClose(driver);
		}


		private void CheckDriverThenClose(RemoteWebDriver driver)
		{
			try
			{
				Assert.IsNotNull(driver);
				driver.Navigate().GoToUrl("http://google.com");
				Log.Debug(GetUserAgent(driver));
			}
			finally
			{
				driver.Quit();
			}

		}

		private string GetUserAgent(RemoteWebDriver driver)
		{
			return driver.ExecuteScript("return window.navigator.userAgent;").ToString();
		}
	}
}
