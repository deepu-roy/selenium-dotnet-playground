using framework.Extensions;
using java.sql;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using sun.security.util;

namespace framework.Pages;

public class LoginPage
{
    //Locators

    private readonly WebDriverWait _wait;
    private readonly IWebDriver _driver;
    private readonly By _loginButton = By.Id("login-button");
    private readonly By _passwordInput = By.Id("password");
    private readonly By _usernameInput = By.Id("user-name");
    private readonly By _errorMessageContainer = By.CssSelector("#login_button_container > div > form > div.error-message-container.error > h3");

    public LoginPage(IWebDriver driver)
    {
        this._driver = driver;
        this._wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
    }

    public void DoLogin(string userName, string password)
    {
        _driver.ClearAndSendKeys(this._usernameInput, userName, _wait);
        _driver.ClearAndSendKeys(this._passwordInput, password, _wait);
        _driver.Click(this._loginButton, _wait);
    }

    public bool IsErrorMessageDisplayed()
    {
        return _driver.Exists(this._errorMessageContainer, _wait);
    }

    public bool IsUserOnLoginPage()
    {
        return _driver.ExistsAndVisible(this._usernameInput, _wait);
    }
}