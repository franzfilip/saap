using DomainModels;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SeleniumBasicUtilities {
    public class BasicScraper : IDisposable, IBasicScraper {

        protected readonly WebDriver driver;

        public BasicScraper(string driverpath) {
            driver = CreateDriver(driverpath);
        }

        public BasicScraper() {
            driver = CreateDriver(CreateOptions());
        }

        private WebDriver CreateDriver(ChromeOptions options) {
            return new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), options.ToCapabilities());
        }

        private ChromeDriver CreateDriver(string driverpath) {
            ChromeOptions options = CreateOptions();

            return new ChromeDriver(driverpath, options);
        }

        private ChromeOptions CreateOptions() {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--no-sandbox");
            options.AddArgument("start-maximized");
            options.AddArgument("disable-infobars");
            options.AddLocalStatePreference("browser", new { excludeSwitches = new string[] { "enable-automation" } });
            options.AddLocalStatePreference("browser", new { useAutomationExtension = false });

            options.AddExcludedArgument("enable-automation");

            ISet<string> DriverArguments = new HashSet<string>();
            DriverArguments.Add("start-maximized");
            DriverArguments.Add("--disable-blink-features");
            DriverArguments.Add("--disable-blink-features=AutomationControlled");
            DriverArguments.Add("disable-infobars");
            DriverArguments.Add("--no-default-browser-check");
            DriverArguments.Add("--no-first-run");

            foreach (string driverArgument in DriverArguments) {
                options.AddArgument(driverArgument);
            }
            options.AddExcludedArgument("enable-automation");

            return options;
        }

        public void Dispose() {
            driver.Quit();
            driver.Dispose();
        }

        public void Navigate(string url) {
            driver.Navigate().GoToUrl(url);
        }

        public void NavigateBack() {
            driver.Navigate().Back();
        }

        public IWebElement FindElement(ByMethod by, string name) {
            return driver.FindElement(GetBy(by, name));
        }

        public List<IWebElement> FindElements(ByMethod by, string name) {
            return driver.FindElements(GetBy(by, name)).ToList();
        }

        public IWebElement FindElement(ByMethod by, string name, IWebElement element) {
            By findBy = GetBy(by, name);
            return element.FindElement(findBy);
        }

        public List<IWebElement> FindElements(ByMethod by, string name, IWebElement element) {
            By findBy = GetBy(by, name);
            return element.FindElements(findBy).ToList();
        }

        public bool WaitUntilElementExists(ByMethod waitUntilElementBy, string waitUntilElementExists) {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(GetBy(waitUntilElementBy, waitUntilElementExists)));
            return element != null;
        }

        public string Read(IWebElement element) {
            return element.Text;
        }

        public string Read(ByMethod by, string elementSelector) {
            IWebElement element = FindElement(by, elementSelector);
            return Read(element);
        }

        public bool TryClickElement(ByMethod byMethod, string elementSelector) {
            try {
                IWebElement element = driver.FindElement(GetBy(byMethod, elementSelector));
                element.Click();
                return true;
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public bool TryClickElement(IWebElement element) {
            try {
                element.Click();
                return true;
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public void Iterate(List<IWebElement> elements) {

        }

        private By GetBy(ByMethod byMethod, string elementSelector) {
            switch (byMethod) {
                case ByMethod.ID:
                    return By.Id(elementSelector);
                case ByMethod.NAME:
                    return By.Name(elementSelector);
                case ByMethod.CLASSNAME:
                    return By.ClassName(elementSelector);
                case ByMethod.TAGNAME:
                    return By.TagName(elementSelector);
                case ByMethod.LINKTEXT:
                    return By.LinkText(elementSelector);
                case ByMethod.PARTIALLINKTEXT:
                    return By.PartialLinkText(elementSelector);
                case ByMethod.CSSSELECTOR:
                    return By.CssSelector(elementSelector);
                case ByMethod.XPATH:
                    return By.XPath(elementSelector);
                default:
                    throw new ArgumentException("Invalid locator method.");
            }
        }
    }
}
