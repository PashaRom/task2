using App.Test.AppConfig;
using App.Entities;
using Task_2_SeleniumWebdriver.Source;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Collections.Generic;

namespace Task_2_SeleniumWebdriver.PageObject
{
    class AutorizationPage
    {
        public AutorizationPage() {
            this.User = Configuration.User;
            this.AutorizationButton = Configuration.Get["pages:autorizationPage:autorizationButton"];
            this.LoginElement = Configuration.Get["pages:autorizationPage:loginElement"];
            this.PasswordElement = Configuration.Get["pages:autorizationPage:passwordElement"];
            this.EnterButtonElement = Configuration.Get["pages:autorizationPage:enterButtonElement"];
            this.CheckLoadElement = Configuration.Get["pages:autorizationPage:chechLoadElement"];
            this.LogOutElement = Configuration.Get["pages:autorizationPage:logOutElement"];
        }

        public User User { get; set; }
        public string LoginElement { get; set; }
        public string AutorizationButton { get; set; }
        public string PasswordElement { get; set; }
        public string EnterButtonElement { get; set; }
        public string CheckLoadElement { get; set; }
        public string LogOutElement { get; set; }

        public void Autorization(ICreatorDriver creatorDriver) {
            IWebElement autorizationButton = creatorDriver.Driver.FindElement(By.XPath(this.AutorizationButton));//".//a[@data-tid='e1c9fd84 2a7436b5']"
            string firstTabHandle = creatorDriver.Driver.CurrentWindowHandle;
            autorizationButton.Click();

            IEnumerable<string> windowHandles = creatorDriver.Driver.WindowHandles;
            foreach (string windowHandle in windowHandles) {
                if (!windowHandle.Equals(firstTabHandle))
                    creatorDriver.Driver.SwitchTo().Window(windowHandle);
            }

            IWebElement inputLogin = creatorDriver.Driver.FindElement(By.CssSelector(this.LoginElement));//"input[name = 'login']"
            inputLogin.SendKeys(this.User.Login);

            IWebElement inputButtonStepLogin = creatorDriver.Driver.FindElement(By.CssSelector(this.EnterButtonElement));//"button[type='submit'][class*='Button2']"
            inputButtonStepLogin.Click();

            IWebElement inputPassword = creatorDriver.Driver.FindElement(By.CssSelector(this.PasswordElement));//"input[id='passp-field-passwd']"
            inputPassword.SendKeys(this.User.Password);
            IWebElement inputButtonStepPassword = creatorDriver.Driver.FindElement(By.CssSelector(this.EnterButtonElement));//"button[type='submit'][class*='Button2']"
            inputButtonStepPassword.Click();
            creatorDriver.Driver.SwitchTo().Window(firstTabHandle);
        }

        public void LogOut(ICreatorDriver creatorDriver) {
            IWebElement logOutElement = creatorDriver.Driver.FindElement(By.CssSelector(this.LogOutElement));
            Actions action = new Actions(creatorDriver.Driver);
            action.MoveToElement(logOutElement).Perform();
            logOutElement.Click();
            IEnumerable<IWebElement> buttonProfileElement = creatorDriver.Driver.FindElements(By.CssSelector("div[data-tid='a3756df5'] button"));
            IReadOnlyCollection<IWebElement> imgAvatar = creatorDriver.Driver.FindElements(By.CssSelector("img[alt='user-avatar']"));
            if(imgAvatar.Count > 0) {
                IReadOnlyCollection<IWebElement> aProfileMenu = creatorDriver.Driver.FindElements(By.CssSelector("a[data-tid='e1c9fd84 f05aebf5 aa5c69b5']"));
                if (aProfileMenu.Count > 0) {
                    foreach (IWebElement a in aProfileMenu) {
                        IWebElement span = a.FindElement(By.CssSelector("span span"));
                        if (span.Text.Equals("Выйти"))
                            a.Click();
                    }
                }
                else {
                    IReadOnlyCollection<IWebElement> imgAvatarNew = creatorDriver.Driver.FindElements(By.CssSelector("img[alt='Аватар']"));
                    if(imgAvatarNew.Count > 0) {
                        foreach(IWebElement img in imgAvatarNew) {
                            img.Click();
                            IReadOnlyCollection<IWebElement> aExits = creatorDriver.Driver.FindElements(By.CssSelector("a[class*='side-menu-item_exit']"));
                            if(aExits.Count > 0) {
                                foreach (IWebElement a in aExits)
                                    a.Click();
                            }

                        }
                    }
                }
             }
            
        }
    }
}
