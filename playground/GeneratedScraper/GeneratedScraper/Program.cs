
using DomainModels;
using GeneratedScraper;
using System.Text.Json;

public class Program {
    public static void Main(string[] args) {
        //string json = "{\r\n  \"ClassName\": \"MyClass\",\r\n  \"Properties\": [\r\n    {\"Name\": \"Property1\", \"Type\": \"string\"},\r\n    {\"Name\": \"Property2\", \"Type\": \"int\"}\r\n  ],\r\n  \"Actions\": [\r\n    {\r\n      \"Kind\": \"NAVIGATE\",\r\n      \"URL\": \"http://www.example.com\",\r\n      \"PropertyPath\": \"\",\r\n      \"SubAction\": null\r\n    }\r\n  ]\r\n}";
        //Input input = JsonSerializer.Deserialize<Input>(json);
        
        ORFScraper scraper = new ORFScraper(null);
        scraper.Start();
    }
}