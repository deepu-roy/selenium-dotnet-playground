using framework.Helper;
using framework.Types;
using OpenDialogWindowHandler;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace framework.Extensions
{
    public static class WebDriverExtensions
    {
        public static void Clear(this IWebDriver driver, By element, WebDriverWait? wait = null)
        {
            if (wait != null)
            {
                wait.Until(ExpectedConditions.ElementExists(element));
                wait.Until(driver => ExpectedConditions.ElementToBeSelected(element));
            }
            var webElement = driver.FindElement(element);
            webElement.SendKeys(Keys.Control + "a");
            webElement.SendKeys(Keys.Delete);
        }

        public static void ClearAndSendKeys(this IWebDriver driver, By element, string value, WebDriverWait? wait = null)
        {
            if (Exists(driver, element, wait))
            {
                var webElement = driver.FindElement(element);
                webElement.Clear();
                webElement.SendKeys(value);
            }
            else
            {
                throw new ElementNotSelectableException();
            }
        }

        public static void Click(this IWebDriver driver, By element, WebDriverWait? wait = null)
        {
            if (driver.ExistsAndClickable(element, wait))
            {
                var webElement = driver.FindElement(element);
                webElement.Click();
            }
            else
            {
                throw new ElementNotInteractableException();
            }
        }

        public static void ClickWithJs(this IWebDriver driver, string locatorForElement, WebDriverWait? wait = null)
        {
            if (wait == null)
            {
                var time = TimeSpan.FromSeconds((double)(double.Parse(ConfigManager.GetConfiguration("implicitWaitTimeout"))));
                wait = new WebDriverWait(driver, time);
            }
            if (locatorForElement.Contains("//"))
            {
                if (driver.Exists(By.XPath(locatorForElement), wait))
                {
                    var scriptToExecute = $"$x(\"{locatorForElement}\")[0].click()";
                    ((IJavaScriptExecutor)driver).ExecuteScript(scriptToExecute);
                }
            }
            else
            {
                if (driver.Exists(By.CssSelector(locatorForElement), wait))
                {
                    var scriptToExecute = $"document.querySelector(\"{locatorForElement}\").click()";
                    ((IJavaScriptExecutor)driver).ExecuteScript(scriptToExecute);
                }
            }
        }

        public static void DragAndDrop(this IWebDriver driver, By source, By target)
        {
            Actions action = new Actions(driver);
            action.DragAndDrop(driver.FindElement(source), driver.FindElement(target)).Build().Perform();
        }

        public static bool Exists(this IWebDriver driver, By element, WebDriverWait? wait)
        {
            if (wait == null)
            {
                var time = TimeSpan.FromSeconds(double.Parse(ConfigManager.GetConfiguration("implicitWaitTimeout")));
                wait = new WebDriverWait(driver, time);
            }
            try
            {
                wait.Until(ExpectedConditions.ElementExists(element));
                var webElement = driver.FindElement(element);
                return webElement?.Displayed ?? false;
            }
            catch
            {
                return false;
            }
        }

        public static void WaitTillUrlIsLike(this IWebDriver driver, string url, WebDriverWait? wait)
        {
            if (wait == null)
            {
                var time = TimeSpan.FromSeconds(double.Parse(ConfigManager.GetConfiguration("implicitWaitTimeout")));
                wait = new WebDriverWait(driver, time);
            }
            wait.Until(ExpectedConditions.UrlContains(url));
        }

        public static bool Exists(this IWebDriver driver, By element, double seconds = 5)
        {
            var time = TimeSpan.FromSeconds((double)(seconds));
            var wait = new WebDriverWait(driver, time);
            try
            {
                wait.Until(ExpectedConditions.ElementExists(element));
                var webElement = driver.FindElement(element);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void ScrollOnTable(this IWebDriver driver, string? cssLocator, bool? isScrollDown = true)
        {
            var counter = 0;
            string direction = isScrollDown == null ? "500" : ((bool)isScrollDown ? "500" : "-500");
            var scriptToExecute = $"var el=document.querySelector('{cssLocator}');el.scrollBy(0,{direction});";
            do
            {
                try
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript(scriptToExecute);
                }
                catch
                {
                    break;
                }
                counter++;
            } while (counter < 3);
        }

        public static bool ExistsAndClickable(this IWebDriver driver, By element, WebDriverWait? wait = null)
        {
            if (wait == null)
            {
                var time = TimeSpan.FromSeconds((double)(double.Parse(ConfigManager.GetConfiguration("implicitWaitTimeout"))));
                wait = new WebDriverWait(driver, time);
            }
            try
            {
                if (wait != null)
                {
                    wait.Until(ExpectedConditions.ElementToBeClickable(element));
                }
                var webElement = driver.FindElement(element);
                return webElement?.Displayed ?? false;
            }
            catch
            {
                return false;
            }
        }

        public static bool ExistsAndVisible(this IWebDriver driver, By element, WebDriverWait? wait = null)
        {
            try
            {
                if (wait != null)
                {
                    wait.Until(ExpectedConditions.ElementIsVisible(element));
                }
                var webElement = driver.FindElement(element);
                return webElement?.Displayed ?? false;
            }
            catch
            {
                return false;
            }
        }

        public static IWebElement FindShadowDomElement(this IWebDriver driver, By shadowParentLocator, By cssLocatorForShadow)
        {
            var shadowRootElement = driver.FindElement(shadowParentLocator);
            var shadowElement = shadowRootElement.GetShadowRoot().FindElement(cssLocatorForShadow);
            return shadowElement;
        }

        public static void HandleBrowseFile(this IWebDriver _driver, By element, string filepath, string filename, WebDriverWait? wait = null)
        {
            Click(_driver, element, wait);
            HandleOpenDialog hndOpen = new HandleOpenDialog();
            hndOpen.fileOpenDialog(filepath, filename);
        }

        public static bool Invisible(this IWebDriver driver, By element, WebDriverWait? wait = null)
        {
            try
            {
                if (wait != null)
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(element));
                }
                var webElement = driver.FindElement(element);
                return !webElement?.Displayed ?? false;
            }
            catch
            {
                return true;
            }
        }

        public static void SearchDropdown(this IWebDriver _driver, By element, string value, WebDriverWait? wait = null)
        {
            ClearAndSendKeys(_driver, element, value, wait);
            Actions action = new Actions(_driver);
            action.SendKeys(Keys.ArrowDown).Perform();
            action.SendKeys(Keys.Enter).Perform();
        }

        public static void SetZoom(this IWebDriver _driver, string zoomLevel)
        {
            WebElement html = (WebElement)_driver.FindElement(By.TagName("html"));

            new Actions(_driver)
                .SendKeys(html, Keys.Control + Keys.Subtract).Build()
                .Perform();
        }
    }
}