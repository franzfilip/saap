// See https://aka.ms/new-console-template for more information
using CodeGenerator;
using Model;
using Newtonsoft.Json;
using System.Text.Json;
using Utility;

internal class Program {
    public static void Main(string[] args) {
        Input input = FileOperations.ReadJson("C:\\FH\\Master\\SAAP\\projectrepo\\saap\\BasicInput.json");
        //DataClassGenerator.GenerateDataClasses(input);
        //ScrapeCodeGenerator scrapeCodeGenerator = new ScrapeCodeGenerator(input);
        //scrapeCodeGenerator.CreateFile();
        CodeFactory codeFactory = new CodeFactory(input);
        codeFactory.GenerateCode();
        Console.WriteLine();
    }
}