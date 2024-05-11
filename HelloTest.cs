using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Selenium_E2E_Tests_TP;

public class SuiteTests : IDisposable
{
    private readonly IWebDriver _driver;
    public SuiteTests()
    {
        _driver = new ChromeDriver();
    }

    public void Dispose()
    {
        _driver.Quit();
    }

    [Fact]
    public void TestHello()
    {
        _driver.Navigate().GoToUrl("https://safatelli.github.io/tp-test-logiciel/assets/hello.html");
        _driver.Manage().Window.Maximize();

        // Test Case 1: Assertion should pass
        SetUsernameAndAssert("Insat", "Bonjour, Insat !");

        // Test Case 2: Assertion should fail
        SetUsernameAndAssert("Arij", "Bonjour, Arij !");
    }

    private void SetUsernameAndAssert(string username, string expectedGreeting)
    {
        _driver.FindElement(By.Id("username")).Clear();
        _driver.FindElement(By.Id("username")).SendKeys(username);
        _driver.FindElement(By.CssSelector("button")).Click();
        
        string actualGreeting = _driver.FindElement(By.CssSelector("p")).Text;
        Assert.Equal(expectedGreeting, actualGreeting);
    }
}