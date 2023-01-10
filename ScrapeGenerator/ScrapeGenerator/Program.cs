// See https://aka.ms/new-console-template for more information
using Model;
using System.Text.Json;
using WebScraper;

internal class Program {
    private static void Main(string[] args) {
        Console.WriteLine("Hello, World!");
        string json = "{\r\n  \"ClassName\": \"MyClass\",\r\n  \"Properties\": [\r\n    {\"Name\": \"Property1\", \"Type\": \"string\"},\r\n    {\"Name\": \"Property2\", \"Type\": \"int\"}\r\n  ],\r\n  \"Actions\": [\r\n    {\r\n      \"Kind\": \"NAVIGATE\",\r\n      \"URL\": \"http://www.example.com\",\r\n      \"PropertyPath\": \"\",\r\n      \"SubAction\": null\r\n    }\r\n  ]\r\n}";

        Input input = JsonSerializer.Deserialize<Input>(json);
        Scraper scraper = new Scraper("C:\\chromedriver2");
    }
}