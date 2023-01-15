using System;
using System.Collections.Generic;
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

            using(BasicScraper scraper = new BasicScraper("C:\\chromedriver2")) {
                scraper.Navigate("https://www.orf.at");
                scraper.WaitUntilElementExists(ByMethod.CLASSNAME, "wrapper");
                scraper.TryClickElement(ByMethod.ID, "didomi-notice-agree-button");

                IterateAndGetMainArticles(data.MainArticles, scraper, () => scraper.FindElements(ByMethod.CSSSELECTOR, "div.oon-grid-top .oon-grid-item"));
                Console.WriteLine();
            }
        }

        public void IterateAndGetMainArticles(List<Article> articles, BasicScraper scraper, Func<List<IWebElement>> getElements) {
            List<IWebElement> elements = getElements();

            for(int i = 0; i < elements.Count; i++) {
                Article article = new Article();
                IWebElement element = elements[i];
                article.URL = element.FindElement(By.CssSelector("a")).GetAttribute("href");
                article.Title = element.Text;
                scraper.TryClickElement(element);
                Thread.Sleep(250);
                article.Description = scraper.Read(ByMethod.CLASSNAME, "story-lead");
                article.Text = scraper.Read(ByMethod.CLASSNAME, "story-content");
                scraper.NavigateBack();
                elements = getElements();
                articles.Add(article);
            }
        }
    }
}
