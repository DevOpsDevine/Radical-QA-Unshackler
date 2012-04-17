namespace  Radical.Infrastructure
{
	/// <summary>
	/// The target browser to use, used in <see cref="IWebStartupSettings.TargetBrowserType"/>.
	/// Local runs may not support all browser versions.
	/// </summary>
    public enum TargetBrowser
    {
		FireFox,
		InternetExplorer,
		GoogleChrome,
		Safari,
		Unknown
    }
}
