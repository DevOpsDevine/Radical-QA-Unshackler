using System;
using System.Collections.Generic;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using log4net;

namespace  Radical.Infrastructure
{
	/// <summary>
	/// Factory class for creating instances of <see cref="RemoteWebDriver"/>
	/// </summary>
	public class WebDriverFactory : IWebDriverFactory
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(WebDriverFactory));
		private readonly IWebStartupSettings _settings;

		protected IWebStartupSettings SeleniumStartup
		{
			get { return _settings; }
		}

		public WebDriverFactory(IWebStartupSettings settings)
		{
			_settings = settings;
		}

		public RemoteWebDriver CreateWebDriver()
		{
			RemoteWebDriver driver = CreateDriver();
			// This sets the "implicit" wait timeout, for finding elements, loading pages, etc.
			driver.Manage().Timeouts().ImplicitlyWait(SeleniumStartup.Timeout);
			return driver;
		}

		public RemoteWebDriver CreateGridDriver()
		{
			return CreateGridDriver(SeleniumStartup);
		}

		public static RemoteWebDriver CreateGridDriver(IWebStartupSettings settings)
		{
			return new RemoteWebDriver(settings.WebDriverHubServerUri, GetCapabilitiesForRemote(settings), settings.Timeout);			
		}

		public static DesiredCapabilities GetCapabilitiesForRemote(IWebStartupSettings settings)
		{
			var desiredCapabilities =  new DesiredCapabilities();
			desiredCapabilities.SetCapability("browserName", settings.BrowserType);

			if (settings.TargetBrowserType == TargetBrowser.GoogleChrome)
			{
				desiredCapabilities.SetCapability("chrome.switches", new List<string> { "--disable-popup-blocking" });
			}

			if(!string.IsNullOrEmpty(settings.BrowserVersion))
			{
				desiredCapabilities.SetCapability("version", settings.BrowserVersion);
			}
			
			return desiredCapabilities;
		}

		private RemoteWebDriver CreateDriver()
		{
			RemoteWebDriver driver;

			if (SeleniumStartup.RunTestsUsingSeleniumGrid)
			{
				driver = CreateGridDriver(SeleniumStartup);
			}
			else
			{
				driver = CreateLocalDriver(SeleniumStartup.TargetBrowserType);
			}

			ReportCapabilities(driver);
			return driver;
		}

		private RemoteWebDriver CreateLocalDriver(TargetBrowser browserType)
		{
			Log.DebugFormat("Creating a local driver of type {0}", browserType);
			RemoteWebDriver driver;
			switch (browserType)
			{
				case TargetBrowser.FireFox:
					driver = new FirefoxDriver(new FirefoxBinary(), new FirefoxProfile(), _settings.Timeout);
					break;
				case TargetBrowser.InternetExplorer:
					driver = new InternetExplorerDriver(_settings.SeleniumServerPort, GetIEOptions(), _settings.Timeout);
					break;
				case TargetBrowser.GoogleChrome:
					driver = new ChromeDriver(_settings.ChromeDriverDirectory, GetChromeOptions(), _settings.Timeout);
					break;
				default:
					throw new NotSupportedException("Cannot create a browser of type " + browserType);
			}
			return driver;
		}

		private ChromeOptions GetChromeOptions()
		{
			var options = new ChromeOptions();
			options.AddArgument("--disable-popup-blocking");
			return options;
		}

		private InternetExplorerOptions GetIEOptions()
		{
			return new InternetExplorerOptions();
		}

		private static void ReportCapabilities(RemoteWebDriver driver)
		{
			Log.InfoFormat("Driver Capabilities: {0}", driver.Capabilities);
			Log.InfoFormat("User agent: {0}", driver.ExecuteScript("return navigator.userAgent;"));
		}
	}
}
