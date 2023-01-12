using DomainModels;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SeleniumBasicUtilities {
    public class BasicScraper : IDisposable, IBasicScraper {

        protected readonly ChromeDriver driver;

        public BasicScraper(string driverpath) {
            driver = CreateDriver(driverpath);
        }

        private ChromeDriver CreateDriver(string driverpath) {
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

            return new ChromeDriver(driverpath, options);
        }

        public void Dispose() {
            driver.Quit();
            driver.Dispose();
        }

        public void Navigate(string url) {
            driver.Navigate().GoToUrl(url);
        }

        public T Read<T>(ByMethod byMethod, string elementSelector, Action<T, string> setPropertyAction) {
            IWebElement element = ReadElementBy(byMethod, elementSelector);
            T instance = (T)Activator.CreateInstance(typeof(T));
            setPropertyAction(instance, element.Text);
            return instance;
        }

        public List<T> ReadMultiple<T>(ByMethod byMethod, string elementSelector, Action<T, string> setPropertyAction) {
            ReadOnlyCollection<IWebElement> elements = ReadElementsBy(byMethod, elementSelector);
            List<T> results = new List<T>();

            foreach (var webElement in elements) {
                T instance = (T)Activator.CreateInstance(typeof(T));
                setPropertyAction(instance, webElement.Text);
                results.Add(instance);
            }

            return results;
        }

        public IWebElement ReadElementBy(ByMethod byMethod, string elementSelector) {
            switch (byMethod) {
                case ByMethod.ID:
                    return driver.FindElement(By.Id(elementSelector));
                case ByMethod.NAME:
                    return driver.FindElement(By.Name(elementSelector));
                case ByMethod.CLASSNAME:
                    return driver.FindElement(By.ClassName(elementSelector));
                case ByMethod.TAGNAME:
                    return driver.FindElement(By.TagName(elementSelector));
                case ByMethod.LINKTEXT:
                    return driver.FindElement(By.LinkText(elementSelector));
                case ByMethod.PARTIALLINKTEXT:
                    return driver.FindElement(By.PartialLinkText(elementSelector));
                case ByMethod.CSSSELECTOR:
                    return driver.FindElement(By.CssSelector(elementSelector));
                case ByMethod.XPATH:
                    return driver.FindElement(By.XPath(elementSelector));
                default:
                    throw new ArgumentException("Invalid locator method.");
            }
        }

        public ReadOnlyCollection<IWebElement> ReadElementsBy(ByMethod byMethod, string elementSelector) {
            switch (byMethod) {
                case ByMethod.ID:
                    return driver.FindElements(By.Id(elementSelector));
                case ByMethod.NAME:
                    return driver.FindElements(By.Name(elementSelector));
                case ByMethod.CLASSNAME:
                    return driver.FindElements(By.ClassName(elementSelector));
                case ByMethod.TAGNAME:
                    return driver.FindElements(By.TagName(elementSelector));
                case ByMethod.LINKTEXT:
                    return driver.FindElements(By.LinkText(elementSelector));
                case ByMethod.PARTIALLINKTEXT:
                    return driver.FindElements(By.PartialLinkText(elementSelector));
                case ByMethod.CSSSELECTOR:
                    return driver.FindElements(By.CssSelector(elementSelector));
                case ByMethod.XPATH:
                    return driver.FindElements(By.XPath(elementSelector));
                default:
                    throw new ArgumentException("Invalid locator method.");
            }
        }
    }
}
