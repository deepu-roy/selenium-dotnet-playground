using framework.Helper;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using OpenQA.Selenium;
using TechTalk.SpecFlow.Infrastructure;

namespace tests.Hooks;

[Binding]
public class TestHooks
{
    public static ExtentReports? Extent;
    private static FeatureContext? _featureContext;
    private static ExtentTest? _featureName;
    private ExtentTest? _currentScenarioName;
    private ScenarioContext _scenarioContext;
    private ISpecFlowOutputHelper _specFlowOutputHelper;

    public TestHooks(FeatureContext _featureContext, ScenarioContext _scenarioContext, ISpecFlowOutputHelper _specFlowOutputHelper)
    {
        this._scenarioContext = _scenarioContext;
        this._specFlowOutputHelper = _specFlowOutputHelper;
        TestHooks._featureContext = _featureContext;
    }

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        var reportPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        var reportName = DateTime.Now.ToString("_MMddyyyy_hhmmtt") + ".html";
        var reportFullName = $"{reportPath}{Path.DirectorySeparatorChar}{reportName}";
        var htmlreporter = new ExtentHtmlReporter(reportFullName);

        htmlreporter.Config.Theme = Theme.Dark;
        Extent = new ExtentReports();
        Extent.AttachReporter(htmlreporter);
    }

    [AfterTestRun]
    public static void ExtentClose()
    {
        Extent?.Flush();
    }

    [AfterScenario]
    public void AfterScenarioCleanup()
    {
        IWebDriver? _driver = null;
        _scenarioContext?.TryGetValue("Driver", out _driver);
        Extent?.Flush();
        _driver?.Dispose();
    }

    [AfterStep]
    public void AfterStep()
    {
        var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();

        IWebDriver? _driver = null;
        _scenarioContext?.TryGetValue("Driver", out _driver);
        var takeScreenshot = Boolean.Parse(ConfigManager.GetConfiguration("takeScreenshot"));
        var takeScreenshotOnlyForFailedStep = Boolean.Parse(ConfigManager.GetConfiguration("takeScreenshotOnlyforFailedStep"));
        MediaEntityModelProvider? mediaEntity = null;
        if ((takeScreenshot && !takeScreenshotOnlyForFailedStep) || (takeScreenshot && takeScreenshotOnlyForFailedStep && _scenarioContext?.TestError != null))
        {
            if (!Directory.Exists("Screenshots"))
            {
                Directory.CreateDirectory("Screenshots");
            }
            var fileName = Path.ChangeExtension($"Screenshots{Path.DirectorySeparatorChar}{Path.GetRandomFileName()}", "png");
            var screenShot = (_driver as ITakesScreenshot)?.GetScreenshot();

            screenShot?.SaveAsFile(fileName);
            _specFlowOutputHelper.AddAttachment(fileName);
            if (screenShot != null)
            {
                var screenShotEncoded = screenShot?.AsBase64EncodedString;
                mediaEntity = MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenShotEncoded, _scenarioContext?.ScenarioInfo.Title.Trim()).Build();
            }
        }

        if (_scenarioContext == null)
            return;

        if (_scenarioContext.TestError == null)
        {
            switch (stepType)
            {
                case "Given":
                    _currentScenarioName?.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Pass("Passed", mediaEntity);
                    break;

                case "When":
                    _currentScenarioName?.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text).Pass("Passed", mediaEntity);
                    break;

                case "And":
                    _currentScenarioName?.CreateNode<And>(_scenarioContext.StepContext.StepInfo.Text).Pass("Passed", mediaEntity);
                    break;

                case "Then":
                    _currentScenarioName?.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text).Pass("Passed", mediaEntity);
                    break;
            }
        }
        if (_scenarioContext.TestError != null)
        {
            switch (stepType)
            {
                case "Given":
                    _currentScenarioName?.CreateNode<Given>(_scenarioContext?.StepContext.StepInfo.Text).Fail(_scenarioContext?.TestError.Message, mediaEntity);
                    break;

                case "When":
                    _currentScenarioName?.CreateNode<When>(_scenarioContext?.StepContext.StepInfo.Text).Fail(_scenarioContext?.TestError.Message, mediaEntity);
                    break;

                case "And":
                    _currentScenarioName?.CreateNode<And>(_scenarioContext?.StepContext.StepInfo.Text).Fail(_scenarioContext?.TestError.Message, mediaEntity);
                    break;

                case "Then":
                    _currentScenarioName?.CreateNode<Then>(_scenarioContext?.StepContext.StepInfo.Text).Fail(_scenarioContext?.TestError.Message, mediaEntity);
                    break;
            }
        }
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        if (_featureContext != null && Extent != null)
            TestHooks._featureName = Extent.CreateTest<Feature>(_featureContext.FeatureInfo.Title);
        this._currentScenarioName = TestHooks._featureName?.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
    }
}