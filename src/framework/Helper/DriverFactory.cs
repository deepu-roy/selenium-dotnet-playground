using framework.Types;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace framework.Helper;

public static class DriverFactory
{
    public static IWebDriver CreateInstance(Browser browserType)
    {
        IWebDriver? driver = null;

        switch (browserType)
        {
            case Browser.Chrome:
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments("--lang=en_US");
                new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
                driver = new ChromeDriver(chromeOptions);
                break;

            case Browser.Edge:
                var edgeOptions = new EdgeOptions();
                new DriverManager().SetUpDriver(new EdgeConfig(), VersionResolveStrategy.MatchingBrowser);
                driver = new EdgeDriver(edgeOptions);
                break;

            case Browser.Firefox:
                new DriverManager().SetUpDriver(new FirefoxConfig(), VersionResolveStrategy.MatchingBrowser);
                driver = new FirefoxDriver();
                break;

            case Browser.Headless:
                new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
                var chromeHeadlessOptions = new ChromeOptions();
                chromeHeadlessOptions.AddArguments("--no-sandbox");
                chromeHeadlessOptions.AddArguments("--headless");
                chromeHeadlessOptions.AddArguments("--lang=en_US");
                chromeHeadlessOptions.AddArguments("window-size=1920,1080");
                chromeHeadlessOptions.AddArguments("disable-gpu");
                driver = new ChromeDriver(chromeHeadlessOptions);

                break;

            default:
                throw new Exception($"Browser is not configured correctly");
        }
        return driver;
    }

    public static IWebDriver CreateInstance(Browser? browserType, string? hubUrl)
    {
        IWebDriver? driver = null;
        if (hubUrl == null)
            hubUrl = @"http://localhost:4444/wd/hub";
        switch (browserType)
        {
            default:
            case Browser.Chrome:
                ChromeOptions chromeOptions = new ChromeOptions();
                driver = GetWebDriver(hubUrl, chromeOptions.ToCapabilities());
                break;

            case Browser.Edge:
                EdgeOptions options = new EdgeOptions();
                driver = GetWebDriver(hubUrl, options.ToCapabilities());
                break;

            case Browser.Firefox:
                FirefoxOptions firefoxOptions = new FirefoxOptions();
                driver = GetWebDriver(hubUrl, firefoxOptions.ToCapabilities());
                break;
        }

        return driver;
    }

    private static IWebDriver GetWebDriver(string hubUrl, ICapabilities capabilities)
    {
        TimeSpan timeSpan = new TimeSpan(0, 3, 0);
        return new RemoteWebDriver(new Uri(hubUrl), capabilities, timeSpan);
    }
}