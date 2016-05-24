using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;

namespace WebDriverAutomationFramework
{
    public class ProvidePageObjectBaseFunctions : IProvidePageObjectBaseFunctions
    {
        private const string DriverLocation = "\\Resources\\Drivers\\";

        public IWebDriver Driver { get; set; }

        internal ProvidePageObjectBaseFunctions()
        {
        }

        internal ProvidePageObjectBaseFunctions(string driver)
        {
            var resourcePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            var parent = Directory.GetParent(resourcePath);

            SetDriver(driver, parent, DriverLocation);
        }

        public void SetDriver(string driver, DirectoryInfo parent, string driverLocation)
        {
            var enumValue = GetEquivalentEnumValue<DriverType>(driver);
            var path = parent + driverLocation;
            switch (enumValue)
            {
                case DriverType.Chrome:
                    Driver = new ChromeDriver(path);
                    break;
                case DriverType.IE:
                    Driver = new InternetExplorerDriver(path);
                    break;
                case DriverType.FireFox:
                    Driver = new FirefoxDriver();
                    break;
                case DriverType.Opera:
                    Driver = new OperaDriver(path);
                    break;
                case DriverType.Safari:
                    Driver = new SafariDriver();
                    break;
                case DriverType.PhantomJS:
                    Driver = new PhantomJSDriver(path);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(6));
        }

        public void NavigateToUrl(Uri url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public void ClickOnElement(string identifier, WebElementType webElementType, TimeSpan timeSpanForWait)
        {
            var webElement = GetElement(identifier, webElementType);
            webElement.Click();
            Thread.Sleep(timeSpanForWait);
        }

        public void ClickOnElement(IWebElement webElement, TimeSpan timeSpanForWait)
        {
            webElement.Click();
            Thread.Sleep(timeSpanForWait);
        }

        public IWebElement GetElement(IWebElement webElement, WebElementType webElementType, string identifier)
        {
            return SearchAndRetrieveElement(SearchType.Element, webElementType, identifier, webElement);
        }

        public IWebElement GetElement(string identifier, WebElementType webElementType)
        {
            return SearchAndRetrieveElement(SearchType.Driver, webElementType, identifier);
        }

        public IEnumerable<IWebElement> GetElements(IWebElement webElement, string identifier, WebElementType webElementType)
        {
            return SearchAndRetrieveElements(SearchType.Element, webElementType, identifier, webElement);
        }

        public IEnumerable<IWebElement> GetElements(string identifier, WebElementType webElementType)
        {
            return SearchAndRetrieveElements(SearchType.Driver, webElementType, identifier);
        }

        public void PerformSubmit(IWebElement webElement, TimeSpan timeSpanForWaiting)
        {
            webElement.Submit();
            Thread.Sleep(timeSpanForWaiting);
        }

        public void ClearAndSendText(IWebElement webElement, string text)
        {
            webElement.Clear();
            webElement.SendKeys(text);
        }

        public void ClearAndSendText(string identifier, WebElementType webElementType, string text)
        {
            var webElement = GetElement(identifier, webElementType);
            webElement.Clear();
            webElement.SendKeys(text);
        }

        public string GetText(string identifier, WebElementType webElementType)
        {
            var element = GetElement(identifier, webElementType);
            return element.Text;
        }

        public object GetAttribute(IWebElement webElement, string identifier, WebElementType webElementType, string attribute)
        {
            var element = GetElement(webElement, webElementType, identifier);
            return element.GetAttribute(attribute);
        }

        public object GetAttribute(string identifier, WebElementType webElementType, string attribute)
        {
            var element = GetElement(identifier, webElementType);
            return element.GetAttribute(attribute);
        }

        public string GetText(IWebElement webElement)
        {
            return webElement.Text;
        }

        public void WaitForLoad(string waitElementIdentifier, WebElementType webElementType, TimeSpan timeInSeconds)
        {
            var wait = new WebDriverWait(Driver, timeInSeconds);
            switch (webElementType)
            {
                case WebElementType.Class:
                    wait.Until(ElementIsVisible(By.ClassName(waitElementIdentifier)));
                    break;
                case WebElementType.Id:
                    wait.Until(ElementIsVisible(By.Id(waitElementIdentifier)));
                    break;
                case WebElementType.PartialLinkText:
                    wait.Until(ElementIsVisible(By.PartialLinkText(waitElementIdentifier)));
                    break;
                case WebElementType.TagName:
                    wait.Until(ElementIsVisible(By.TagName(waitElementIdentifier)));
                    break;
                case WebElementType.Name:
                    wait.Until(ElementIsVisible(By.Name(waitElementIdentifier)));
                    break;
                case WebElementType.CssSelector:
                    wait.Until(ElementIsVisible(By.CssSelector(waitElementIdentifier)));
                    break;
                case WebElementType.LinkText:
                    wait.Until(ElementIsVisible(By.LinkText(waitElementIdentifier)));
                    break;
                case WebElementType.Xpath:
                    wait.Until(ElementIsVisible(By.XPath(waitElementIdentifier)));
                    break;
                default:
                    throw new Exception("The specified WebElementType was not found.");
            }
        }

        public object GetMatchingPropertyName(string name, Type instance)
        {
            var properties = instance.GetProperties();
            var firstOrDefault = properties.FirstOrDefault(prop => string.Equals(prop.Name, name, StringComparison.CurrentCultureIgnoreCase));
            if (firstOrDefault != null)
            {
                return firstOrDefault.GetValue(instance);
            }
            throw new Exception("no such property wit the identifier:" + name + " could be found in the instance:" + instance);
        }

        private static Func<IWebDriver, IWebElement> ElementIsVisible(By locator)
        {
            return driver =>
            {
                try
                {
                    return ElementIfVisible(driver.FindElement(locator));
                }
                catch (StaleElementReferenceException)
                {
                    return null as IWebElement;
                }
            };
        }

        private static IWebElement ElementIfVisible(IWebElement element)
        {
            return (!element.Displayed || !element.Enabled) ? null : element;
        }

        private IEnumerable<IWebElement> SearchAndRetrieveElements(SearchType searchType, WebElementType webElementType, string identifier, ISearchContext webElement = null)
        {
            switch (searchType)
            {
                case SearchType.Driver:
                    return RetrieveElements(Driver, webElementType, identifier);
                case SearchType.Element:
                    return RetrieveElements(webElement, webElementType, identifier);
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
            }
        }

        private static IEnumerable<IWebElement> RetrieveElements(ISearchContext searchContext, WebElementType webElementType, string identifier)
        {
            switch (webElementType)
            {
                case WebElementType.Class:
                    return searchContext.FindElements(By.ClassName(identifier));
                case WebElementType.Id:
                    return searchContext.FindElements(By.Id(identifier));
                case WebElementType.CssSelector:
                    return searchContext.FindElements(By.CssSelector(identifier));
                case WebElementType.LinkText:
                    return searchContext.FindElements(By.LinkText(identifier));
                case WebElementType.Name:
                    return searchContext.FindElements(By.Name(identifier));
                case WebElementType.PartialLinkText:
                    return searchContext.FindElements(By.PartialLinkText(identifier));
                case WebElementType.TagName:
                    return searchContext.FindElements(By.TagName(identifier));
                case WebElementType.Xpath:
                    return searchContext.FindElements(By.XPath(identifier));
                default:
                    throw new ArgumentOutOfRangeException(nameof(webElementType), webElementType, null);
            }
        }

        private IWebElement SearchAndRetrieveElement(SearchType searchType, WebElementType webElementType, string identifier, ISearchContext webElement = null)
        {
            switch (searchType)
            {
                case SearchType.Driver:
                    return RetrieveElement(Driver, webElementType, identifier);
                case SearchType.Element:
                    return RetrieveElement(webElement, webElementType, identifier);
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
            }
        }

        private static IWebElement RetrieveElement(ISearchContext searchContext, WebElementType webElementType, string identifier)
        {
            switch (webElementType)
            {
                case WebElementType.Class:
                    return searchContext.FindElement(By.ClassName(identifier));
                case WebElementType.Id:
                    return searchContext.FindElement(By.Id(identifier));
                case WebElementType.PartialLinkText:
                    return searchContext.FindElement(By.PartialLinkText(identifier));
                case WebElementType.TagName:
                    return searchContext.FindElement(By.TagName(identifier));
                case WebElementType.Name:
                    return searchContext.FindElement(By.Name(identifier));
                case WebElementType.CssSelector:
                    return searchContext.FindElement(By.CssSelector(identifier));
                case WebElementType.LinkText:
                    return searchContext.FindElement(By.LinkText(identifier));
                case WebElementType.Xpath:
                    return searchContext.FindElement(By.XPath(identifier));
                default:
                    throw new Exception("The specified WebElementType was not found.");
            }
        }

        private static T GetEquivalentEnumValue<T>(string driverName)
        {
            return (T)Enum.Parse(typeof(T), driverName, true);
        }
    }
}