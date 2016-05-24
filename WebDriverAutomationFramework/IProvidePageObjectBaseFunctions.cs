using System;
using System.Collections.Generic;
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

        IWebElement GetElement(IWebElement webElement, WebElementType webElementType, string identifier);

        IWebElement GetElement(string identifier, WebElementType webElementType);

        IEnumerable<IWebElement> GetElements(string identifier, WebElementType webElementType);

        IEnumerable<IWebElement> GetElements(IWebElement webElement, string identifier, WebElementType webElementType);

        void WaitForLoad(string identifier, WebElementType webElementType, TimeSpan fromSeconds);

        void ClearAndSendText(string identifier, WebElementType webElementType, string text);

        void ClearAndSendText(IWebElement webElement, string text);

        void PerformSubmit(IWebElement button, TimeSpan timeSpanForWaiting);

        object GetMatchingPropertyName(string name, Type type);

        string GetText(string identifier, WebElementType webElementType);

        string GetText(IWebElement webElement);

        object GetAttribute(IWebElement webElement, string identifier, WebElementType webElementType, string attribute);

        object GetAttribute(string identifier, WebElementType webElementType, string attribute);
    }
}