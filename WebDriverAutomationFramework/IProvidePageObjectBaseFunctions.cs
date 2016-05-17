using System;
using System.IO;
using OpenQA.Selenium;

namespace WebDriverAutomationFramework
{
    public interface IProvidePageObjectBaseFunctions
    {
        IWebDriver Driver { get; set; }

        void SetDriver(string driver, DirectoryInfo parent, string driverLocation);

        void NavigateToUrl(Uri uri);

        void ClickOnElement(string identifier, WebElementType webElementType, TimeSpan timeforWaiting);

        void ClickOnElement(IWebElement webElement, TimeSpan timeforWaiting);

        IWebElement FindElementByElement(IWebElement webElement, WebElementType webElementType, string identifier);

        IWebElement FindElementByLocator(string identifier, WebElementType webElementType);

        void WaitForLoad(string identifier, TimeSpan fromSeconds, WebElementType webElementType);

        void ClearAndSendText(string identifier, WebElementType webElementType, string text);

        void ClearAndSendText(IWebElement webElement, string text);

        void PerformSubmit(IWebElement button, TimeSpan timeSpanForWaiting);

        object GetAppropriateName(string name, Type type);

        string GetText(string identifier, WebElementType webElementType);

        string GetText(IWebElement webElement);
    }
}