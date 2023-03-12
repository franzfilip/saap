//using DomainModels;
//using OpenQA.Selenium;
//using SeleniumBasicUtilities;
//using System;

//namespace GeneratedScraper {
//    public class ORFScraper {
//        private readonly Input input;
//        public ORFScraper(Input input) {
//            this.input = input;
//        }

//        public void Start() {
//            Data data = new Data();

//            using(BasicScraper scraper = new BasicScraper("C:\\chromedriver2")) {
//                GoToORFAndAcceptCookies(scraper);
//                data.MainArticles = IterateAndGetMainArticles(scraper, () => scraper.FindElements(ByMethod.CSSSELECTOR, "div.oon-grid-top .oon-grid-item"));
//            }
//        }

//        //Go to URL and accept cookies
//        public void GoToORFAndAcceptCookies(IBasicScraper scraper) {
//            scraper.Navigate("https://www.orf.at");
//            scraper.WaitUntilElementExists(ByMethod.CLASSNAME, "wrapper");
//            scraper.TryClickElement(ByMethod.ID, "didomi-notice-agree-button");
//        }

//        public List<Article> IterateAndGetMainArticles(BasicScraper scraper, Func<List<IWebElement>> getElements) {
//            List<IWebElement> elements = getElements();
//            List<Article> articles = new List<Article>();

//            for(int i = 0; i < elements.Count; i++) {
//                Article article = new Article();
//                IWebElement element = elements[i];
//                article.URL = element.FindElement(By.CssSelector("a")).GetAttribute("href");
//                article.Title = scraper.Read(element);
//                scraper.TryClickElement(element);
//                Thread.Sleep(250);
//                article.Description = scraper.Read(ByMethod.CLASSNAME, "story-lead");
//                article.Text = scraper.Read(ByMethod.CLASSNAME, "story-content");
//                scraper.NavigateBack();
//                elements = getElements();
//                articles.Add(article);
//            }

//            return articles;
//        }
//    }
//}

using DomainModels;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using SeleniumBasicUtilities;
namespace GeneratedScraper {
    public class ORFScraper {
        protected readonly WebDriver driver;

        public ORFScraper() {
            driver = CreateDriver(CreateOptions());
        }

        private WebDriver CreateDriver(ChromeOptions options) {
            return new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), options.ToCapabilities());
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
    }
}