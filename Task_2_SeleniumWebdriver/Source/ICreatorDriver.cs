using OpenQA.Selenium;
using App.Entities;

namespace Task_2_SeleniumWebdriver.Source
{
    public interface ICreatorDriver
    {
        public IWebDriver CreateDriver();
        public IWebDriver Driver { get; }  
        public TypeWebDriver TypeWebDriver { get; }
       
    }
}
