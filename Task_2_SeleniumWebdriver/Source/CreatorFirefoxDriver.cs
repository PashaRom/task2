using System;
using App.Test.AppConfig;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using App.Entities;

namespace Task_2_SeleniumWebdriver.Source
{
    public class CreatorFirefoxDriver : ICreatorDriver
    {
        IWebDriver webDriver;
        private readonly string path;
        private readonly string name;
        private readonly TypeWebDriver type;

        public CreatorFirefoxDriver() {
            path = Configuration.Get["browsers:firefox:pathGeckoDriver"];
            name = "FirefoxDriver";
        }
        public IWebDriver CreateDriver() {

            webDriver = new FirefoxDriver(path);
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            return webDriver;
        }
        private int GetImplicitWait()
        {
            string configImplicitWait = Configuration.Get["browsers:firefox:options:implicitWait"];
            int implicitWait = 0;
            if (!String.IsNullOrEmpty(configImplicitWait))
                return Convert.ToInt32(configImplicitWait);
            return implicitWait;
        }

        public IWebDriver Driver {
            get {
                return this.webDriver;
            }
        }
        public override string ToString()
        {
            return name;
        }

        public TypeWebDriver TypeWebDriver {
            get {
                return this.type;
            }
        }
    }
}
