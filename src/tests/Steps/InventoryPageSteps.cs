using framework.Pages;
using OpenQA.Selenium;

namespace Aditro.L.UI.Tests.Steps;

[Binding]
public class InventoryPageSteps
{
    private IWebDriver _driver;
    private FeatureContext _featureContext;
    private ScenarioContext _scenarioContext;

    public InventoryPageSteps(ScenarioContext scenarioContext, FeatureContext featureContext)
    {
        _scenarioContext = scenarioContext;
        _featureContext = featureContext;
        _scenarioContext.TryGetValue("Driver", out _driver);
    }

    [Then(@"the user is navigated to Inventory Page")]
    public void ThenUserIsOnTheHomePage()
    {
        bool isUserOnInventoryPage = new InventoryPage(this._driver).IsUserOnInventoryPage();
        Assert.True(isUserOnInventoryPage, "User is not on inventory page");
    }

    [Then(@"the product images are all the same")]
    public void ThenTheProductImagesAreAllTheSame()
    {
        bool isProductImagesTheSame = new InventoryPage(this._driver).AreProductImagesUnique();
        Assert.True(isProductImagesTheSame, "Problem user is shown valid images");
    }
}