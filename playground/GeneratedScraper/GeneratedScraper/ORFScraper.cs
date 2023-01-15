using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DomainModels;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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


                //Iterate()
                //List<Article> mainArticles = scraper.ReadMultiple<Article>(mainArticlesElements, (a, s) => a.Title = s);
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
                article.Description = scraper.Read<Article>(ByMethod.CLASSNAME, "story-lead", (a, s) => a.Description = s).Description;
                article.Text = scraper.Read<Article>(ByMethod.CLASSNAME, "story-content", (a, s) => a.Text = s).Text;
                scraper.NavigateBack();
                elements = getElements();
                articles.Add(article);
            }
        }
    }
}
