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
			}
		}
		//Go to URL and accept cookies
		private void GoToORFAndAcceptCookies()
		{
			scraper.Navigate("https://www.orf.at");
			scraper.WaitUntilElementExists(ByMethod.CLASSNAME, "wrapper");
		}
		//Save all main articles of the website
		private List<Article> SaveMainArticles()
		{
			data.MainArticles = IterateAndGetMainArticles(scraper, () => scraper.FindElements(ByMethod.CSSSELECTOR, "div.oon-grid-top .oon-grid-item"));
			asdf
			scraper.TryClickElement(element);
			Thread.Sleep(250);
			asdf
		}
	}
}