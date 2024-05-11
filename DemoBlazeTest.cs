using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Selenium_E2E_Tests_TP.Data;
using SeleniumExtras.WaitHelpers;

namespace Selenium_E2E_Tests_TP;

public class DemoBlazeTests : IDisposable
{
    private readonly IWebDriver _driver;
    private const string homeUrl = "https://www.demoblaze.com/";

    public DemoBlazeTests()
    {
        _driver = new ChromeDriver();
        _driver.Manage().Window.Maximize();
        _driver.Navigate().GoToUrl(homeUrl);
    }

    public void Dispose()
    {
        _driver.Quit();
    }

    [Fact]
    public void TestPhoneCategoryLoadsCorrectProduct()
    {
        _driver.FindElement(By.LinkText("Phones")).Click();  // Assuming 'Phones' is the text link for navigating to the phone category

        // Use WebDriverWait to wait until at least one product card is loaded
        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        wait.Until(drv => drv.FindElement(By.ClassName("h-100")).Displayed);

        // Assert that the correct product is displayed
        var productName = _driver.FindElement(By.CssSelector(".card-title a")).Text;
        var productPrice = _driver.FindElement(By.CssSelector(".card h5")).Text;
        var productDescription = _driver.FindElement(By.CssSelector("#article")).Text;

        Assert.Equal(DemoBlazeTestData.ProductName, productName);
        Assert.Equal(DemoBlazeTestData.ProductPrice, productPrice);
        Assert.Contains(DemoBlazeTestData.ProductDescription, productDescription);
    }
    
    [Fact]
    public void TestAddingProductToCart()
    {
        _driver.FindElement(By.LinkText("Phones")).Click();
        // Use WebDriverWait to wait until at least one product card is loaded
        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        wait.Until(drv => drv.FindElement(By.ClassName("h-100")).Displayed);
        // Navigate to a product page
        _driver.FindElement(By.LinkText(DemoBlazeTestData.ProductName)).Click();
        Console.WriteLine("Navigated to product page");
        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a.btn.btn-success.btn-lg")));
        _driver.FindElement(By.CssSelector("a.btn.btn-success.btn-lg")).Click();


        try {
            var alert = _driver.SwitchTo().Alert();
            Assert.Equal("Product added", alert.Text);
            alert.Accept();
        } catch (NoAlertPresentException ex) {
            Console.WriteLine("Alert not present: " + ex.Message);
        }
        
    }
    
    [Fact]
    public void TestCartPage()
    {
        TestAddingProductToCart();
        _driver.FindElement(By.LinkText("Cart")).Click();
        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));  // Increased timeout for better reliability

        // Ensure the cart table is visible and has loaded
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".table")));

        // Additional check for the presence of specific product details
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".table tbody tr td:nth-child(2)")));
    
        var productName = _driver.FindElement(By.CssSelector(".table tbody tr td:nth-child(2)")).Text;

        Assert.Equal("Samsung galaxy s6", productName);

    }

    [Fact]
    public void TestCheckoutProcess()
    {
        // Add a product to the cart and go to checkout
        TestCartPage();

        // Click on 'Place Order' button
        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        IWebElement placeOrderButton =
            _driver.FindElement(By.CssSelector("button.btn.btn-success[data-target='#orderModal']"));
        placeOrderButton.Click();

        // Wait for the modal to appear
        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("orderModal")));

        // Now fill out the form within the modal
        _driver.FindElement(By.Id("name")).SendKeys(DemoBlazeTestData.UserName);
        _driver.FindElement(By.Id("country")).SendKeys(DemoBlazeTestData.Country);
        _driver.FindElement(By.Id("city")).SendKeys(DemoBlazeTestData.City);
        _driver.FindElement(By.Id("card")).SendKeys(DemoBlazeTestData.CardNumber);
        _driver.FindElement(By.Id("month")).SendKeys(DemoBlazeTestData.Month);
        _driver.FindElement(By.Id("year")).SendKeys(DemoBlazeTestData.Year);
        _driver.FindElement(By.CssSelector("#orderModal .btn-primary")).Click();

        // Click the primary button to submit the form
        _driver.FindElement(By.CssSelector("#orderModal .btn-primary")).Click();

        // Verify if the order confirmation is visible or check for confirmation text
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".sweet-alert")));
        string confirmationText = _driver.FindElement(By.CssSelector(".sweet-alert h2")).Text;
        Assert.Equal(DemoBlazeTestData.ExpectedConfirmationMessage, confirmationText);

        // Optionally, close the alert if needed
        _driver.FindElement(By.CssSelector(".sweet-alert .confirm")).Click();
    }
    [Fact]
    public void NavigateToAboutUs() {
        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementIsVisible(By.LinkText("About us")));
        _driver.FindElement(By.LinkText("About us")).Click();
        _driver.FindElement(By.ClassName("modal-content"));
        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("videoModalLabel")));
        var title = _driver.FindElement(By.Id("videoModalLabel")).Text;
        Assert.Equal( "About us",title);
    }
    
}