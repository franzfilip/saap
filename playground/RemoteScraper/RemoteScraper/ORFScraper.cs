﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace GeneratedScraper {
    public class ORFScraper: IDisposable {

        protected readonly WebDriver driver;

        //public ORFScraper() {
        //    driver = CreateDriver(CreateOptions());
        //}

        private RemoteWebDriver CreateDriver(ChromeOptions options) {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddAdditionalChromeOption("BrowserVersion", "74");
            chromeOptions.AddAdditionalChromeOption("PlatformName", "Windows 10");

            return new RemoteWebDriver(new Uri("http://localhost:4444"), chromeOptions.ToCapabilities());
        }

        private ChromeOptions CreateOptions() {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--no-sandbox");
            options.AddArgument("start-maximized");
            options.AddArgument("disable-infobars");
            options.AddLocalStatePreference("browser", new { excludeSwitches = new string[] { "enable-automation" } });
            options.AddLocalStatePreference("browser", new { useAutomationExtension = false });

            options.AddExcludedArgument("enable-automation");

            ISet<string> driverArguments = new HashSet<string>
            {
                "start-maximized",
                "--disable-blink-features",
                "--disable-blink-features=AutomationControlled",
                "disable-infobars",
                "--no-default-browser-check",
                "--no-first-run"
            };

            foreach (string driverArgument in driverArguments) {
                options.AddArgument(driverArgument);
            }
            options.AddExcludedArgument("enable-automation");

            return options;
        }

        public void Start() {
            try {
                ChromeOptions chromeOptions = new ChromeOptions();
                chromeOptions.BrowserVersion = "110.0";
                chromeOptions.AddArgument("--no-sandbox");
                chromeOptions.AddArgument("start-maximized");
                chromeOptions.AddArgument("disable-infobars");
                chromeOptions.AddLocalStatePreference("browser", new { excludeSwitches = new string[] { "enable-automation" } });
                chromeOptions.AddLocalStatePreference("browser", new { useAutomationExtension = false });

                chromeOptions.AddExcludedArgument("enable-automation");

                ISet<string> DriverArguments = new HashSet<string>();
                DriverArguments.Add("start-maximized");
                DriverArguments.Add("--disable-blink-features");
                DriverArguments.Add("--disable-blink-features=AutomationControlled");
                DriverArguments.Add("disable-infobars");
                DriverArguments.Add("--no-default-browser-check");
                DriverArguments.Add("--no-first-run");

                foreach (string driverArgument in DriverArguments)
                {
                    chromeOptions.AddArgument(driverArgument);
                }
                chromeOptions.AddExcludedArgument("enable-automation");
                //chromeOptions.AddAdditionalChromeOption("browserVersion", "110");
                //chromeOptions.AddAdditionalChromeOption("platformName", "Windows 10");

                using (var driver = new RemoteWebDriver(new Uri("http://localhost:4444"), chromeOptions.ToCapabilities())) {
                    GoToORFAndAcceptCookies(driver);
                    SaveMainArticles(driver, null);
                    //data.MainArticles = SaveMainArticles(scraper, () => scraper.FindElements(ByMethod.CSSSELECTOR, "div.oon-grid-top .oon-grid-item"));
                }
            }
            catch(Exception ex) {
                Console.WriteLine("An exception occurred:");
                Console.WriteLine("Message: " + ex.Message);
                Console.WriteLine("Stack trace: " + ex.StackTrace);
                Console.WriteLine("Source: " + ex.Source);
                Console.WriteLine("Inner exception: " + ex.InnerException);
                Console.WriteLine("Target site: " + ex.TargetSite);
                Dispose();
                throw ex;
            }
        }

        private void GoToORFAndAcceptCookies(RemoteWebDriver driver) {
            driver.Navigate().GoToUrl("https://www.orf.at");
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.ClassName("wrapper")));
            Thread.Sleep(250);
            element = driver.FindElement(By.Id("didomi-notice-agree-button"));
            element.Click();
            //scraper.TryClickElement(ByMethod.ID, "didomi-notice-agree-button");
        }
        //Save all main articles of the website
        private void SaveMainArticles(IWebDriver scraper, Func<List<IWebElement>> getElements) {
            List<IWebElement> elements = scraper.FindElements(By.ClassName("oon-grid-item")).ToList();
            foreach(var element in elements)
            {
                Console.WriteLine(element.Text);
            }
        }

        public void Dispose()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}