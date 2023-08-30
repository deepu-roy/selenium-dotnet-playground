using framework.Types;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace framework.Helper;

public partial class TestBase : IDisposable
{
    public IWebDriver Driver;

    public TestBase()
    {
        ConfigManager.Configure();
        TestDataManager.Configure();
        Driver = GetDriver();
    }

    // Making sure the driver is disposed at the end of each test
    public void Dispose()
    {
        Driver.Quit();
    }

    private IWebDriver GetDriver()
    {
        Browser browser = (Browser)Enum.Parse(typeof(Browser), ConfigManager.GetConfiguration("browser"));
        TimeSpan timeOut = new TimeSpan(0, 0, int.Parse(ConfigManager.GetConfiguration("implicitWaitTimeout")));
        if (ConfigManager.GetConfiguration("useHub")?.ToLower() == "true")
        {
            Driver = DriverFactory.CreateInstance(browser, ConfigManager.GetConfiguration("hubUrl"));
        }
        else
        {
            Driver = DriverFactory.CreateInstance(browser);
        }

        // Setting implicit time out for the browser
        Driver.Manage().Timeouts().ImplicitWait = timeOut;
        Driver.Manage().Window.Maximize();
        return Driver;
    }
}