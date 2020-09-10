using System;
using App.Test.AppConfig;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Drawing;
using Task_2_SeleniumWebdriver.Source;

namespace Task_2_SeleniumWebdriver.PageObject
{
    public class HomePage
    {
        public HomePage() {            
            this.Url = Configuration.Get["pages:homePage:url"];
            this.CheckLocator = Configuration.Get["pages:homePage:chechLoadElement"];
            
            this.NumberCategories = GetNumberCategoties();
        }
        public string Url { get; set; }
        public string CheckLocator { get; set; }
        
        public string SearchLocator { get; set; }
        public int NumberCategories { get; set; }

        public List<IWebElement> GetListTopCatigories(ICreatorDriver creatorDriver) {
            List<IWebElement> divMenuDisplayElement = new List<IWebElement>();
            IWebElement divMenuElement = creatorDriver.Driver.FindElement(By.CssSelector("div[role='tablist'] div[data-tid='f2d7f3b0']"));
            IReadOnlyCollection<IWebElement> aMenuCategoriesElement = divMenuElement.FindElements(By.CssSelector("div[data-zone-name='category-link']>div[data-tid='acbe4a11 f2d7f3b0']>a"));//[class='_3Lwc_UVFq4']          
            Point locationMenuElement = new Point { X = -9999, Y = -9999 };
            foreach (IWebElement element in aMenuCategoriesElement)
            {
                if (locationMenuElement.X == -9999)
                    locationMenuElement = element.Location;
                Point location = element.Location;
                if (element.Displayed && element.Enabled && locationMenuElement.Y == location.Y)
                {
                    divMenuDisplayElement.Add(element);
                }
            }
            return divMenuDisplayElement;
        }

        public string GetCatigory(string href) {
            string[] splitAtrHref = href.Split('/');
            return splitAtrHref[splitAtrHref.Length - 1];
        }

        private int GetNumberCategoties() {
            string confNumberCatigoryes = Configuration.Get["pages:homePage:numberCategories"];
            return Convert.ToInt32(confNumberCatigoryes);
        }

        public IEnumerable<IWebElement> GetAllCategories(ICreatorDriver creatorDriver) {
            IWebElement buttonAllCategoriesElement0 = creatorDriver.Driver.FindElement(By.CssSelector("button[id='27903767-tab']"));
            IReadOnlyCollection<IWebElement> buttonElements = creatorDriver.Driver.FindElements(By.CssSelector("button[data-tid='e1c9fd84 5a689c45']"));
            IWebElement buttonAllCategoriesElement1 = null;
            foreach (IWebElement button in buttonElements)
                buttonAllCategoriesElement1 = creatorDriver.Driver.FindElement(By.CssSelector("button[data-tid='e1c9fd84 5a689c45']"));
            if (buttonAllCategoriesElement0.Displayed)
                buttonAllCategoriesElement0.Click();
            if (buttonAllCategoriesElement1 != null)
            {
                if (buttonAllCategoriesElement1.Displayed)
                    buttonAllCategoriesElement1.Click();
            }
            IWebElement allCategoriesElement = creatorDriver.Driver.FindElement(By.CssSelector("div[role='tablist'][data-tid='2bd21f10 668cc030'][aria-orientation='vertical']"));//
            IReadOnlyCollection<IWebElement> aAllCategoriesElement = allCategoriesElement.FindElements(By.CssSelector("div[data-zone-name='category-link']>button>a"));//div[data-tid='acbe4a11 f2d7f3b0']
            return aAllCategoriesElement;
        }

        public IEnumerable<string> GetIdAllCategories(ICreatorDriver creatorDriver) {
            IEnumerable<IWebElement> aAllCategoriesElement = GetAllCategories(creatorDriver);
            List<string> idAllCategories = new List<string>();
            foreach (IWebElement element in aAllCategoriesElement) {
                string id = GetCatigory(element.GetAttribute("href"));
                idAllCategories.Add(id);                
            }
            return idAllCategories;
        }
    }
}
