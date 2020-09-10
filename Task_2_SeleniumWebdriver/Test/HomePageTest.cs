using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using Task_2_SeleniumWebdriver.Source;
using Task_2_SeleniumWebdriver.PageObject;
using App.Test.TestEntities;
using App.Test.File;

namespace Task_2_SeleniumWebdriver.Test
{
    [TestFixture]
    class HomePageTest {
        
        HomePage homePage;
        AutorizationPage autorizationPage;
        List<string> idCromeTopCategories;
        List<string> idFirefoxTopCategories;

        [OneTimeSetUp]
        public void OneTimeSetUp() {
            homePage = new HomePage();
            autorizationPage = new AutorizationPage();
            idCromeTopCategories = new List<string>();
            idFirefoxTopCategories = new List<string>();
            foreach (ICreatorDriver creatorDriver in TestWebDrivers.CreatorDrivers) { 
                creatorDriver.CreateDriver();
                creatorDriver.Driver.Url = homePage.Url;
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() {
            foreach (ICreatorDriver creatorDriver in TestWebDrivers.CreatorDrivers) {
                creatorDriver.Driver.Close();
                creatorDriver.Driver.Quit();
            }
        }

        [TestCaseSource(typeof(TestWebDrivers), "CreatorDrivers"),
            Order(1)]
        
        public void LoadHomePage(ICreatorDriver creatorDriver) {
            Assert.DoesNotThrow(()=> creatorDriver.Driver.FindElement(By.XPath(homePage.CheckLocator)), 
                $"Home page ${homePage.Url} has not loaded or a html element with {homePage.CheckLocator} atributes has not found on the page ${homePage.Url}");
        }
        [TestCaseSource(typeof(TestWebDrivers), "CreatorDrivers"),
            Order(2)]
        public void Autorization(ICreatorDriver creatorDriver) {
            autorizationPage.Autorization(creatorDriver);
            Assert.DoesNotThrow(()=> creatorDriver.Driver.FindElement(By.CssSelector(autorizationPage.CheckLoadElement)),
                $"Authorization has been invalid.");
        }
        [TestCaseSource(typeof(TestWebDrivers), "CreatorDrivers"),
            Order(3)]
        public void GetListTopCatigories(ICreatorDriver creatorDriver) {
            List<IWebElement> aElements = homePage.GetListTopCatigories(creatorDriver);
            List<string> idTopCategories = new List<string>();
            foreach(IWebElement element in aElements) {
                string id = homePage.GetCatigory(element.GetAttribute("href"));
                idTopCategories.Add(id);
            }
            string pathToCsvFile = String.Format("{1}\\{0}-top-categories.csv", creatorDriver.ToString(), App.Test.AppConfig.Configuration.DirToCsvFile);
            WorkWithFile<string>.WritingCsv(pathToCsvFile, idTopCategories);
            Assert.IsTrue(homePage.GetListTopCatigories(creatorDriver).Count > 0,
                $"Top menu elements had not found on page {homePage.Url}.");
        }
        [TestCaseSource(typeof(TestWebDrivers), "CreatorDrivers"),
            Order(5)]
        public void GoToRendomMenuItem(ICreatorDriver creatorDriver) {
            List<IWebElement> aMenuElement = homePage.GetListTopCatigories(creatorDriver);            
            Random random = new Random();
            int numberRendom = random.Next(0, aMenuElement.Count - 1);
            IWebElement rendomMenuElement = aMenuElement[numberRendom];
            string attrHref = rendomMenuElement.GetAttribute("href");
            string numberCategory = homePage.GetCatigory(attrHref); 
            rendomMenuElement.Click();
            IWebElement checkElemen = creatorDriver.Driver.FindElement(By.CssSelector("html head meta[property='og:url']"));
            string categoryContent = checkElemen.GetAttribute("content");
            string checkNumberCategory = homePage.GetCatigory(categoryContent);
            Assert.AreEqual(numberCategory, checkNumberCategory);
        }

        [TestCaseSource(typeof(TestWebDrivers), "CreatorDrivers"),
            Order(6)]
        public void BackToHomePage(ICreatorDriver creatorDriver) {
            creatorDriver.Driver.Navigate().Back();
            Assert.DoesNotThrow(() => creatorDriver.Driver.FindElement(By.XPath(homePage.CheckLocator)),
                $"Home page ${homePage.Url} has not loaded or a html element with {homePage.CheckLocator} atributes has not found on the page ${homePage.Url} after click Back button in browser");
        }
        [TestCaseSource(typeof(TestWebDrivers), "CreatorDrivers"),
            Order(7)]
        public void GetAllCategoriesAndExportToCsv(ICreatorDriver creatorDriver) {
            
            IEnumerable<IWebElement> aAllCategoriesElement = homePage.GetAllCategories(creatorDriver);//creatorDriver.Driver.FindElements(By.CssSelector("div[data-zone-name='category-link']>div[data-tid='acbe4a11 f2d7f3b0']>a"));
            
            List<string> categoryNames = new List<string>();
            IEnumerable<string> idAllCategories = homePage.GetIdAllCategories(creatorDriver);
            foreach (IWebElement aElement in aAllCategoriesElement) {                
                string name = aElement.FindElement(By.CssSelector("a span")).Text;                
                categoryNames.Add(name);
            }            
            string pathToCsvFile = String.Format("{1}\\{0}-categories.csv", creatorDriver.ToString(),App.Test.AppConfig.Configuration.DirToCsvFile);
            string pathToCsvFileForAllCategories = String.Format("{1}\\{0}-id-categories.csv", creatorDriver.ToString(), App.Test.AppConfig.Configuration.DirToCsvFile);
            WorkWithFile<string>.WritingCsv(pathToCsvFile, categoryNames);
            WorkWithFile<string>.WritingCsv(pathToCsvFileForAllCategories, idAllCategories);
            Assert.AreEqual(homePage.NumberCategories, categoryNames.Count,
                $"Expected number of categories {homePage.NumberCategories} but actual value was {categoryNames.Count}");
        }
        [TestCaseSource(typeof(TestWebDrivers), "CreatorDrivers"),
            Order(8)]
        public void CompareTopCategoriesWitnAllCategories(ICreatorDriver creatorDriver) {            
            string pathToCsvFile = String.Format("{1}\\{0}-top-categories.csv", creatorDriver.ToString(), App.Test.AppConfig.Configuration.DirToCsvFile);
            string pathToIDCsvFile = String.Format("{1}\\{0}-id-categories.csv", creatorDriver.ToString(), App.Test.AppConfig.Configuration.DirToCsvFile);
            IEnumerable<string> idTopCategories = WorkWithFile<string>.ReadingCsv(pathToCsvFile);
            IEnumerable<string> idAllCategoties = WorkWithFile<string>.ReadingCsv(pathToIDCsvFile);                                          //IEnumerable<String> allCategories = WorkWithFile<string>.ReadingCsv(pathToCsvFile);
            foreach (string nameCategory in idTopCategories)
                CollectionAssert.Contains(idAllCategoties, nameCategory,
                    $"Top category product item {nameCategory} has not found in list all categories.");
        }

        [TestCaseSource(typeof(TestWebDrivers), "CreatorDrivers")]
        public void LogOut(ICreatorDriver creatorDriver) {
            autorizationPage.LogOut(creatorDriver);

            Assert.DoesNotThrow(() => creatorDriver.Driver.FindElement(By.XPath(autorizationPage.AutorizationButton)));
        }

    }
}
