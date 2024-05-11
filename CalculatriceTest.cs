using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Selenium_E2E_Tests_TP;

public class CalculatorTests : IDisposable
{
    private readonly IWebDriver _driver;

    public CalculatorTests()
    {
        _driver = new ChromeDriver();
    }

    public void Dispose()
    {
        _driver.Quit();
    }

    [Fact]
    public void TestAddition()
    {
        SetupCalculator("10", "+", "5");
        AssertResult("Résultat : 15");
    }

    [Fact]
    public void TestSubtraction()
    {
        SetupCalculator("10", "-", "5");
        AssertResult("Résultat : 5");
    }

    [Fact]
    public void TestMultiplication()
    {
        SetupCalculator("10", "*", "5");
        AssertResult("Résultat : 50");
    }

    [Fact]
    public void TestDivision()
    {
        SetupCalculator("10", "/", "5");
        AssertResult("Résultat : 2");
    }

    [Fact]
    public void TestDivisionByZero()
    {
        SetupCalculator("10", "/", "0");
        AssertResult("Résultat : Infinity");
    }
    
    private void SetupCalculator(string num1, string operation, string num2)
    {
        _driver.Navigate().GoToUrl("https://safatelli.github.io/tp-test-logiciel/assets/calc.html");
        _driver.Manage().Window.Maximize();
        _driver.FindElement(By.Id("num1")).SendKeys(num1);
        _driver.FindElement(By.Id("operator")).SendKeys(operation);
        _driver.FindElement(By.Id("num2")).SendKeys(num2);
        _driver.FindElement(By.CssSelector("button")).Click();
    }

    private void AssertResult(string expected)
    {
        var result = _driver.FindElement(By.Id("result")).Text;
        Assert.Equal(expected, result);
    }
}