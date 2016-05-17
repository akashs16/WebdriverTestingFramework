using System;
using NUnit.Framework;
using WebDriverAutomationFramework;

namespace WebDriverFramworkTests
{
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
    }
}
