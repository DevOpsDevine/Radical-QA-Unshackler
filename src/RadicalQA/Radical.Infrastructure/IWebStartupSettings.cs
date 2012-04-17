using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace  Radical.Infrastructure
{
	/// <summary>
	/// Configuration values required to start Selenium. <see cref="LocalSeleniumServerProxy"/>
	/// </summary>
	public interface IWebStartupSettings
	{
		/// <summary>
		/// The browser to use.
		/// 
		/// </summary>
		/// <remarks>
		/// When using Selenium, this is sent to the browserCommand of <see cref="DefaultSelenium"/>
		/// 
		/// When using <see cref="RemoteWebDriver"/>, this becomes <see cref="DesiredCapabilities.BrowserName"/>
		/// </remarks>
		string BrowserType { get; }

		/// <summary>
		/// The browser version. 
		/// </summary>
		/// <remarks>
		/// If not empty, this should be something included in the userAgent reported by the browser.
		/// </remarks>
		string BrowserVersion { get; }

		/// <summary>
		/// This is derived from the <see cref="BrowserType"/>
		/// </summary>
		TargetBrowser TargetBrowserType { get; }
		
		/// <summary>
		/// The first page to load when starting the test suite.
		/// </summary>
		string RootUrl { get;  } 

		/// <summary>
		/// Directory of the Selenium server ("selenium-server.jar")
		/// </summary>
		string SeleniumServerDirectory { get;  }

		/// <summary>
		/// Selenium server to use e.g. "selenium-server.jar", "selenium-server-standalone-2.3.0.jar"
		/// </summary>
		string SeleniumJar { get; }

		/// <summary>
		/// The timeout for Selenium commands in miliseconds
		/// </summary>
		int TimeoutMilliseconds { get; }

		/// <summary>
		/// The timeout for commands that load normal pages. 
		/// </summary>
		/// <remarks>
		/// This is used as the command timeout when creating a <see cref="RemoteWebDriver"/>
		/// The <see cref="WebDriverFactory"/> sets implicit waits to this value. <see cref="ITimeouts.ImplicitlyWait"/>
		/// </remarks>
		TimeSpan Timeout { get; }

		/// <summary>
		/// A timeout to use for AJAX operations that may assume that the page is “warm” and the action will not re-render the entire page. 
		/// </summary>
		TimeSpan AjaxTimeout { get; }

		/// <summary>
		/// Timeout for purely client side actions. For any action that interacts with the server, use <see cref="AjaxTimeout"/>
		/// </summary>
		TimeSpan ClientSideTimeout { get; }

		/// <summary>
		/// Initial height of browser window (in pixels).
		/// </summary>
		int BrowserHeight { get; }

		/// <summary>
		/// Initial width of browser window (in pixels).
		/// </summary>
		int BrowserWidth { get; }

		/// <summary>
		/// Run tests using the grid
		/// </summary>
		bool RunTestsUsingSeleniumGrid { get; }

		/// <summary>
		/// The Se hub server to use. 
		/// This will be used for both remote webdrivers and Selenium remotes.
		/// </summary>
		string SeleniumHubServerName { get; }

		/// <summary>
		/// Web driver hub server uri.
		/// </summary>
		Uri WebDriverHubServerUri { get;  }

		/// <summary>
		/// The port used by <see cref="SeleniumHubServerName"/>
		/// </summary>
		int SeleniumServerPort { get; }

		/// <summary>
		/// When starting a selenium server collect server logs. Only applies to local Selenium processes.
		/// </summary>
		bool CollectSeleniumLogs { get;  }

		/// <summary>
		/// Name of the server that provides the computer's name.
		/// If <see cref="WhoAmIServer"/> is not defined, <see cref="WhoAmIUri"/> uses <see cref="SeleniumHubServerName"/>
		/// </summary>
		string WhoAmIServer { get; }

		/// <summary>
		/// Uri to the service that will provide the computers name. 
		/// If <see cref="WhoAmIServer"/> is not defined, <see cref="WhoAmIUri"/> uses <see cref="SeleniumHubServerName"/>
		/// </summary>
		Uri WhoAmIUri { get; }

		/// <summary>
		/// Location of chromedriver.exe. 
		/// This will almost always be found automatically.
		/// </summary>
		/// <remarks>
		/// This setting is only required because we want a constructor for <see cref="ChromeDriver"/> that takes a timeout.
		/// The <see cref="ChromeDriver(string, ICapabilities, TimeSpan)">ChromeDriver constructor</see> that takes a timeout requires the directory of the chrome driver.
		/// </remarks>
		string ChromeDriverDirectory { get;  }

		/// <summary>
		/// If acquiring a WedDriver context fails, retry for up to x seconds.
		/// Works in conjunction with AcquireContentRetryInterval
		/// </summary>
		int AcquireContextTimeoutSeconds { get; }

		/// <summary>
		/// Retry to acquire a WebDriver context every x seconds.
		/// Works in conjunction with AcquireContextTimeoutSeconds
		/// </summary>
		int AcquireContextIntervalSeconds { get; }
	}
}