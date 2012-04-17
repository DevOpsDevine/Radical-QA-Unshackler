using OpenQA.Selenium.Remote;

namespace Radical.Infrastructure
{
	/// <summary>
	/// Provides <see cref="RemoteWebDriver"/> instances used to drive the UI.
	/// </summary>
	public interface IWebDriverFactory
	{
		RemoteWebDriver CreateWebDriver();
	}
}