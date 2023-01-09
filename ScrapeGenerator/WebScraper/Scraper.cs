using Model;
using OpenQA.Selenium.Chrome;

namespace WebScraper {
    public class Scraper : IDisposable, IBasicScraper {

        protected readonly ChromeDriver driver;

        public Scraper() {
            driver = CreateDriver();
        }

        private ChromeDriver CreateDriver() {
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

            return new ChromeDriver("C:\\chromedriver2", options);
        }

        public void Dispose() {
            driver.Quit();
            driver.Dispose();
        }

        public void Navigate(string url) {
            driver.Navigate().GoToUrl(url);
        }

        public void Read<T>(ByMethod byMethod, string elementSelector, Action<T> setPropertyAction) {
            throw new NotImplementedException();
        }
    }
}