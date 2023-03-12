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
				GoToORFAndAcceptCookies(scraper);
				SaveMainArticles(scraper);
			}
		}
		//Go to URL and accept cookies
		private void GoToORFAndAcceptCookies()
		{
			scraper.Navigate("https://www.orf.at");
			scraper.WaitUntilElementExists(ByMethod.CLASSNAME, "wrapper");
			scraper.TryClickElement(ByMethod.ID, "didomi-notice-agree-button");
			Thread.Sleep(250);
		}
		//Save all main articles of the website
		private List<Article> SaveMainArticles()
		{
			List<IWebElement> elements = getElements();
			List<Article> collected = new();
			for(int i = 0; i < elements.Count; i++) {
				Article article = new Article();
				IWebElement element = elements[i];
				article.Title = scraper.Read(element);
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