using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace  Radical.Infrastructure
{
	public class WebStartupSettings : IWebStartupSettings 
	{
		
		public const string DefaultWebRoot = "http://localhost";
		public const int DefaultSeleniumServerPort = 4444;
		
		public const int DefaultBrowserWidth = 1024;
		public const int DefaultBrowserHeight = 768;
		public const string DefaultSeleniumJar = "selenium-server.jar";
		public const string HubServerRelativerUrl = "wd/hub";
		private const string ChromedriverExe = "chromedriver.exe";
		
		public const int DefaultTimeoutSeconds = 30;
		private const int DefaultClientSideTimeoutSeconds = 2;
		private const int DefaultAjaxTimeoutSeconds = 8;

		public WebStartupSettings()
		{
			BrowserType = "googlechrome";
			TimeoutSeconds = DefaultTimeoutSeconds;
			SeleniumJar = DefaultSeleniumJar;
			SeleniumServerDirectory = "";
			BrowserWidth = DefaultBrowserWidth;
			BrowserHeight = DefaultBrowserHeight;
			RunTestsUsingSeleniumGrid = false;
			SeleniumServerPort = DefaultSeleniumServerPort;
			WebRoot = DefaultWebRoot;
			ChromeDriverDirectory = FindChromeDriverDirectory();
			ClientSideTimeout = TimeSpan.FromSeconds(DefaultClientSideTimeoutSeconds);
			AjaxTimeout = TimeSpan.FromSeconds(DefaultAjaxTimeoutSeconds);
	    	AcquireContextIntervalSeconds = 20;
	    	AcquireContextTimeoutSeconds = 300;
		}

		/// <summary>
		/// The timeout for Selenium commands in milliseconds.
		/// </summary>
		public int TimeoutMilliseconds
		{
			get
			{
				return (int)Timeout.TotalMilliseconds;
			}
		}

		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromSeconds(TimeoutSeconds);
			}
		}

		public TimeSpan AjaxTimeout
		{
			get; set;
		}

		public TimeSpan ClientSideTimeout
		{
			get;set;
		}

		/// <summary>
		/// The timeout for Selenium commands in seconds.
		/// </summary>
		public int TimeoutSeconds
		{
			get;set;			
		}

		/// <summary>
		/// Selenium <see cref="TargetBrowser"/>
		/// 
		/// When using the grid <see cref="RunTestsUsingSeleniumGrid"/> this must be a value that will match a grid remote
		/// "internet explorer"
		/// "firefox"
		/// "chrome"
		/// </summary>
		/// <remarks>
		/// When local, this is passed as the browsercommand to <see cref="DefaultSelenium"/>
		/// </remarks>
		/// <example>
		/// *firefox [absolute path]
		/// Automatically launch a new Firefox process using a custom Firefox profile. This profile will be automatically configured to use the Selenium Server as a proxy and to have all annoying prompts ("save your password?" "forms are insecure" "make Firefox your default browser?" disabled. You may optionally specify an absolute path to your firefox executable, or just say "*firefox". If no absolute path is specified, we'll look for firefox.exe in a default location (normally c:\program files\mozilla firefox\firefox.exe), which you can override by setting the Java system property 
		/// </example>
		/// <example>
		/// *iexplore [absolute path]
		///- Automatically launch a new Internet Explorer process using custom Windows registry settings. This process will be automatically configured to use the Selenium Server as a proxy and to have all annoying prompts ("save your password?" "forms are insecure" "make Firefox your default browser?" disabled. You may optionally specify an absolute path to your iexplore executable, or just say "*iexplore". If no absolute path is specified, we'll look for iexplore.exe in a default location (normally c:\program files\internet explorer\iexplore.exe), which you can override by setting the Java system property 
		/// </example>
		public string BrowserType
		{
			get; set;
		}

		/// <summary>
		/// This is derived from the <see cref="BrowserType"/>
		/// </summary>
		public TargetBrowser TargetBrowserType
		{
			get { return GetBrowserType(); }
		}

		/// <summary>
		/// The browser version. 
		/// </summary>
		/// <remarks>
		/// If not empty, this should be something included in the userAgent reported by the browser.
		/// </remarks>
		public string BrowserVersion
		{
			get;set;
		}

		/// <summary>
		/// Selenium server to use e.g. "selenium-server.jar", "selenium-server-standalone-2.3.0.jar"
		/// </summary>
		public string SeleniumJar
		{
			get;set;
		}
		
		/// <summary>
		/// Directory of the Selenium server ("selenium-server.jar")
		/// </summary>
		public string SeleniumServerDirectory
		{
			get;set;
		}

		/// <summary>
		/// Initial height of browser window (in pixels).
		/// </summary>
		public int BrowserHeight
		{
			get;set;
		}

		/// <summary>
		/// Initial width of browser window (in pixels).
		/// </summary>
		public int BrowserWidth
		{
			get;set;
		}

		/// <summary>
		/// Run tests using the grid
		/// </summary>
		public bool RunTestsUsingSeleniumGrid
		{
			get;set;
		}

		/// <summary>
		/// The Se hub server to use. 
		/// This will be used for both remote webdrivers and Selenium remotes.
		/// </summary>
		public string SeleniumHubServerName
		{
			get { return HubServerName;  }
		}

		public string HubServerName { get; set; }

		/// <summary>
		/// The port used by <see cref="SeleniumHubServerName"/>
		/// </summary>
		public int SeleniumServerPort
		{
			get;set;
		}

		/// <summary>
		/// When starting a selenium server collect server logs. Only applies to local Selenium processes.
		/// </summary>
		public bool CollectSeleniumLogs
		{
			get;set;
		}

		/// <summary>
		/// The first page to load when starting selenium.
		/// </summary>
		public virtual string RootUrl
		{
			get { return WebRoot;  }
		}

		public string WebRoot { get; set; }

		public Uri WebDriverHubServerUri
		{
			get
			{
				var serverName = SeleniumHubServerName;
				if(String.IsNullOrEmpty(serverName))
				{
					return null;
				}

				return new Uri(String.Format(@"http://{0}:{1}/{2}", SeleniumHubServerName, SeleniumServerPort, HubServerRelativerUrl));
			}
		}

		public string WhoAmIServer
		{
			get; set;
		}

		public Uri WhoAmIUri
		{
			get
			{
				var whoAmI = String.IsNullOrEmpty(WhoAmIServer) ? SeleniumHubServerName : WhoAmIServer;
				return new Uri(String.Format(@"http://{0}", whoAmI));
			}
		}

		public string ChromeDriverDirectory
		{
			get; set;
		}

		public int AcquireContextTimeoutSeconds { get; set; }

		public int AcquireContextIntervalSeconds { get; set; }

		private TargetBrowser GetBrowserType()
		{
			TargetBrowser targetBrowser;
			string targetBrowserString = BrowserType;
			if (Enum.TryParse(targetBrowserString, true, out targetBrowser))
			{
				return targetBrowser;
			}

			targetBrowserString = targetBrowserString.ToLower();

			if (targetBrowserString.Contains("firefox") || targetBrowserString.Contains("ff"))
			{
				return TargetBrowser.FireFox;
			}
			if (targetBrowserString.Contains("explore") || targetBrowserString.Contains("ie"))
			{
				return TargetBrowser.InternetExplorer;
			}
			if (targetBrowserString.Contains("chrome"))
			{
				return TargetBrowser.GoogleChrome;
			}
			if (targetBrowserString.Contains("safari"))
			{
				return TargetBrowser.Safari;
			}

			return TargetBrowser.Unknown;
		}

		/// <summary>
		/// Gets a path where chromedriver lives.
		/// </summary>
		private static string FindChromeDriverDirectory()
		{
			// On my system this is: C:\Program Files (x86)\Java\jre6\bin
			var found = GetSearchPaths().Where(File.Exists).ToList();
			if (found.Count > 0)
			{
				return new FileInfo(found.First()).Directory.FullName;
			}

			return string.Empty;
		}

		private static IEnumerable<string> GetSearchPaths()
		{
			var paths = Environment.GetEnvironmentVariable("PATH").Split(';');

			var exeFolder = new string[] 
			                	{
			                		GetExecutingAssembly()
			                	};

			return
				from t in exeFolder.AsQueryable()
				from p in paths.AsQueryable()
				select Path.Combine(Path.Combine(p, t), ChromedriverExe);

		}

		private static string GetExecutingAssembly()
		{
			return GetCurrentExecutingDirectory(Assembly.GetExecutingAssembly());
		}

		/// <summary>
		/// Gets the current assemblies current location (pre-shadow copy).
		/// </summary>
		/// <remarks>
		/// Under ASP.NET this will return the bin directory.
		/// </remarks>
		/// <returns>The full local directory path</returns>
		public static string GetCurrentExecutingDirectory(Assembly assembly)
		{
			return CreateLocalWindowsPath(assembly.CodeBase);
		}

		/// <summary>
		/// Turns a file Uri into a local windows path.
		/// file://c:/somedir/somefile.txt -> c:\somedir\somefile.txt
		/// </summary>
		/// <param name="fileUriPath">Full Uri file path</param>
		/// <returns>The Windows friendly path</returns>
		private static string CreateLocalWindowsPath(string fileUriPath)
		{
			string filePath = new Uri(fileUriPath).LocalPath;
			return Path.GetDirectoryName(filePath);
		}
	}
}
