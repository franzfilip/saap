
using GeneratedScraper;
using System.Text.Json;
using System.Threading.Tasks;

public class Program {
    public static void Main(string[] args) {
        ORFScraper scraper = new ORFScraper();
        ORFScraper scraper2 = new ORFScraper();
        ORFScraper scraper3 = new ORFScraper();
        //scraper.Start();
        Task task1 = Task.Run(() => scraper.Start());
        Task task2 = Task.Run(() => scraper2.Start());
        Task task3 = Task.Run(() => scraper3.Start());

        Task.WaitAll(task1, task2, task3);
        //ORFScraper scraper2 = new ORFScraper();
        //scraper2.Start();
    }
}