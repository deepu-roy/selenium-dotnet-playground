namespace framework.Helper;

public static class ExceptionHandler
{
    public static void Handle(Exception e)
    {
        switch (e.GetType().ToString())
        {
            case "OpenQA.Selenium.ElementNotSelectableException":
                {
                    Console.WriteLine("Test Failed. ElementNotSelectableException occured");
                    throw e;
                }
            case "OpenQA.Selenium.ElementNotInteractableException":
                {
                    Console.WriteLine("Test Failed. ElementNotInteractableException occured");
                    throw e;
                }
            case "OpenQA.Selenium.ElementNotVisibleException":
                {
                    Console.WriteLine("Test Failed. ElementNotVisibleException occured");
                    throw e;
                }
            case "OpenQA.Selenium.NoSuchFrameException":
                {
                    Console.WriteLine("Test Failed. NoSuchFrameException occured");
                    throw e;
                }
            case "OpenQA.Selenium.NoAlertPresentException":
                {
                    Console.WriteLine("Test Failed. NoAlertPresentException occured");
                    throw e;
                }
            case "OpenQA.Selenium.NoSuchWindowException":
                {
                    Console.WriteLine("Test Failed. NoSuchWindowException occured");
                    throw e;
                }
            case "OpenQA.Selenium.StaleElementReferenceException":
                {
                    Console.WriteLine("Test Failed. StaleElementReferenceException occured");
                    throw e;
                }
            case "OpenQA.Selenium.SessionNotFoundException":
                {
                    Console.WriteLine("Test Failed. SessionNotFoundException");
                    throw e;
                }
            case "OpenQA.Selenium.TimeoutException":
                {
                    Console.WriteLine("Test Failed. TimeoutException occured");
                    throw e;
                }
            case "OpenQA.Selenium.WebDriverException":
                {
                    Console.WriteLine("Test Failed. WebDriverException occured");
                    throw e;
                }
            default:
                {
                    Console.WriteLine("Test Failed. Exception occured");
                    throw e;
                }
        }
    }
}