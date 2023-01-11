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
            using(BasicScraper scraper = new BasicScraper("C:\\chromedriver2")) {
                scraper.Navigate("https://www.orf.at");
            }
        }
    }
}
