namespace WebDriverFramworkTests
{
    using System;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using WebDriverAutomationFramework;

    [TestFixture]
    public class ProvidePageObjectBaseFunctionsTests
    {
        [TestCase("chrome")]
        [TestCase("firefox")]
        [TestCase("ie")]
        public void WhenITryToConsumeTheFrameworkPackageIShouldBeAbleToDoThat(string driverName)
        {
            // arrange
            var factory = new BaseOperationsFactory();
            var baseFunctions = factory.Create(driverName);

            // act
            var url = new Uri("https://www.google.com");
            baseFunctions.NavigateToUrl(url);
            var driver = baseFunctions.Driver;

            // assert
            var title = driver.Title;
            try
            {
                Assert.That(title, Is.EqualTo("Google"));
            }

            //cleanup
            finally
            {
                driver.Quit();
            }
        }


        [TestCase("chrome")]
        [TestCase("firefox")]
        public void TheWaitForElementLoadShouldWorkAsExpected(string driverName)
        {
            // arrange
            var factory = new BaseOperationsFactory();
            var baseFunctions = factory.Create(driverName);
            const string elementIdentifier = "sbico";

            // act
            var url = new Uri("https://www.google.com");
            baseFunctions.NavigateToUrl(url);
            try
            {
                baseFunctions.GetElement(elementIdentifier, WebElementType.Id);
                Assert.Fail("The elemet is already visible");
            }
            catch (WebDriverException)
            {
            }

            baseFunctions.ClearAndSendText("lst-ib", WebElementType.Id, "Anarki");

            // assert
            try
            {
                baseFunctions.WaitForLoad(elementIdentifier, WebElementType.Class, TimeSpan.FromSeconds(3));
                baseFunctions.ClickOnElement(elementIdentifier, WebElementType.Class, TimeSpan.Zero);
            }
            catch (WebDriverException e)
            {
                Assert.Fail("The element is visible even after wait time; exception: " + e);
            }

            //cleanup
            finally
            {
                baseFunctions.Driver.Quit();
            }
        }


        [TestCase("chrome")]
        [TestCase("firefox")]
        public void TheWaitForElementUnloadShouldWorkAsExpected(string driverName)
        {
            // arrange
            var factory = new BaseOperationsFactory();
            var baseFunctions = factory.Create(driverName);
            const string elementIdentifier = "#tsf > div.tsf-p > div.jsb > center > input[type=\"submit\"]:nth-child(1)";

            // act
            var url = new Uri("https://www.google.com");
            baseFunctions.NavigateToUrl(url);

            try
            {
                baseFunctions.GetElement(elementIdentifier, WebElementType.CssSelector);
            }
            catch (WebDriverException e)
            {
                Assert.Fail("The element is already hidden or not aviable on the page exeption: " + e);
            }

            baseFunctions.ClearAndSendText("lst-ib", WebElementType.Id, "Anarki");

            // assert
            try
            {
                baseFunctions.WaitForUnload(elementIdentifier, WebElementType.CssSelector, TimeSpan.FromSeconds(3), true);
            }
            catch (WebDriverException e)
            {
                Assert.Fail("The element was still visible even when it shouldn't be exception: " + e);
            }

            //cleanup
            finally
            {
                baseFunctions.Driver.Quit();
            }
        }
    }
}
