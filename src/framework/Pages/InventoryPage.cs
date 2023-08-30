using framework.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace framework.Pages;

public class InventoryPage
{
    // Locators

    private readonly WebDriverWait _wait;
    private readonly By _inventoryHeader = By.CssSelector("#header_container > div.header_secondary_container > span");
    private readonly By _productImages = By.CssSelector("div.inventory_item_img img");
    private IWebDriver _driver;

    public InventoryPage(IWebDriver driver)
    {
        this._driver = driver;
        this._wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
    }

    public bool IsUserOnInventoryPage()
    {
        return _driver.ExistsAndVisible(this._inventoryHeader, _wait);
    }

    public bool AreProductImagesUnique()
    {
        var allImageElements = _driver.FindElements(this._productImages);
        string imageSource = string.Empty;
        imageSource = allImageElements.ElementAt(0).GetAttribute("src");

        foreach (var imageElement in allImageElements)
        {
            if (imageSource != imageElement.GetAttribute("src")) return false;
        }
        return true;
    }
}