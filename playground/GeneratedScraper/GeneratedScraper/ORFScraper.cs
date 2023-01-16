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
using SeleniumBasicUtilities;
namespace GeneratedScraper {
    public class ORFScraper {
        private readonly Input input;
        public ORFScraper(Input input) {
            this.input = input;
        }
        public void Start() {
            Data data = new Data();
            using (BasicScraper scraper = new BasicScraper("C:\\chromedriver3")) {
                GoToORFAndAcceptCookies(scraper);
                data.MainArticles = SaveMainArticles(scraper, () => scraper.FindElements(ByMethod.CSSSELECTOR, "div.oon-grid-top .oon-grid-item"));
            }
        }
        //Go to URL and accept cookies
        private void GoToORFAndAcceptCookies(BasicScraper scraper) {
            scraper.Navigate("https://www.orf.at");
            scraper.WaitUntilElementExists(ByMethod.CLASSNAME, "wrapper");
            Thread.Sleep(250);
            scraper.TryClickElement(ByMethod.ID, "didomi-notice-agree-button");
        }
        //Save all main articles of the website
        private List<Article> SaveMainArticles(BasicScraper scraper, Func<List<IWebElement>> getElements) {
            List<IWebElement> elements = getElements();
            List<Article> collected = new();
            for (int i = 0; i < elements.Count; i++) {
                Article article = new Article();
                IWebElement element = elements[i];
                article.Title = scraper.Read(element);
                scraper.TryClickElement(element);
                Thread.Sleep(250);
                article.Description = scraper.Read(ByMethod.CLASSNAME, "story-lead");
                article.Text = scraper.Read(ByMethod.CLASSNAME, "story-content");
                scraper.NavigateBack();
                elements = getElements();
                collected.Add(article);
            }
            return collected;
        }
    }
}