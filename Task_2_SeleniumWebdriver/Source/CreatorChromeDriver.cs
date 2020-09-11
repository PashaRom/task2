using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using App.Test.AppConfig;
using App.Entities;

namespace Task_2_SeleniumWebdriver.Source
{
    public class CreatorChromeDriver : ICreatorDriver
    {
        private IWebDriver webDriver;
        private readonly string path;
        private readonly string name;
        private readonly TypeWebDriver type;
        public CreatorChromeDriver() {
            path = Configuration.Get["browsers:chrome:pathChromeDriver"];
            if (path == null)
                throw new Exception($"Has not got or invalided path \"{path}\" to CromeDriver in {Configuration.NameConfigFile}.");
            name = "ChromeDriver";
            type = TypeWebDriver.CromeDriver;
        }

        public IWebDriver CreateDriver() {
            webDriver = new ChromeDriver(
                path,
                InitChromeOptions());
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(GetImplicitWait());
            return webDriver;
        }
        public IWebDriver Driver {
            get {
                return webDriver;
            }
        }
        private ChromeOptions InitChromeOptions() {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.LeaveBrowserRunning = GetLeaveBrowserRunningOption();
            string startSizeWindow = Configuration.Get["browsers:chrome:options:startSizeWindow"];
            if (startSizeWindow != null)
                chromeOptions.AddArgument(startSizeWindow);
            string mode = Configuration.Get["browsers:chrome:options:mode"];
            if(mode != null)
                chromeOptions.AddArgument(mode);
            return chromeOptions;
        }
        private bool GetLeaveBrowserRunningOption() {
            string configLeaveBrowserRunning = Configuration.Get["browsers:chrome:options:leaveBrowserRunning"];
            bool leaveBrowserRunning = false;
            if (configLeaveBrowserRunning.Equals("true"))            
                leaveBrowserRunning = true;            
            return leaveBrowserRunning;
        }  
        
        private int GetImplicitWait() {
            string configImplicitWait = Configuration.Get["browsers:chrome:options:implicitWait"];
            int implicitWait = 0;
            if (!String.IsNullOrEmpty(configImplicitWait))
                return Convert.ToInt32(configImplicitWait);
            return implicitWait;
        }
        public override string ToString() {
            return name;
        }
        public TypeWebDriver TypeWebDriver {
            get {
                return this.type;
            }
        }
    }
}
