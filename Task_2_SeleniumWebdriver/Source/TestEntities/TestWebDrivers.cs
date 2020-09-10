using System.Collections.Generic;
using Task_2_SeleniumWebdriver.Source;

namespace App.Test.TestEntities
{
    public class TestWebDrivers
    {
        public static CreatorChromeDriver creatorChromeDriver;
        public static CreatorFirefoxDriver creatorFirefoxDriver;

        static TestWebDrivers() {
            creatorChromeDriver = new CreatorChromeDriver();
            creatorFirefoxDriver = new CreatorFirefoxDriver();
        }

        public static IEnumerable<ICreatorDriver> CreatorDrivers {
            get {
                yield return creatorChromeDriver;
                yield return creatorFirefoxDriver;
            }
        }

    }
}
