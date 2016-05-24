﻿using OpenQA.Selenium;

namespace WebDriverAutomationFramework
{
    public class BaseOperationsFactory
    {
        public IProvidePageObjectBaseFunctions Create()
        {
            return new ProvidePageObjectBaseFunctions();
        }

        public IProvidePageObjectBaseFunctions Create(string driver)
        {
            return new ProvidePageObjectBaseFunctions(driver);
        }

        public IProvidePageObjectBaseFunctions Create(IWebDriver driver)
        {
            return new ProvidePageObjectBaseFunctions(driver);
        }
    }
}
