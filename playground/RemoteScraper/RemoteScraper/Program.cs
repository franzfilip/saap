
using GeneratedScraper;
using System.Text.Json;

public class Program {
    public static void Main(string[] args) {
        ORFScraper scraper = new ORFScraper();
        scraper.Start();
    }
}