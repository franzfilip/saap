using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Utility;
using static System.Collections.Specialized.BitVector32;

namespace CodeGenerator {
    public class ScrapeCodeGenerator {
        private List<string> fileContent = new();
        private readonly Input input;
        private readonly List<MethodToGenerate> methodsToGenerate;
        public ScrapeCodeGenerator(Input input, List<MethodToGenerate> methodsToGenerate) {
            this.input = input;
            this.methodsToGenerate = methodsToGenerate;
        }

        public void CreateFile() {
            AddUsings();
            CreateNameSpace();
            string fileString = String.Join("\n", fileContent);
            FileOperations.WriteToFile(fileString, input.Path, "ORFScraper.cs");
        }

        private void AddUsings() {
            fileContent.Add("using DomainModels;");
            fileContent.Add("using OpenQA.Selenium;");
            fileContent.Add("using SeleniumBasicUtilities;");
        }

        private void CreateNameSpace() {
            fileContent.Add($"namespace {input.Namespace} {{");
            CreateClass(1);
            fileContent.Add("}");
        }

        private void CreateClass(int tabs) {
            fileContent.Add($"public class ORFScraper {{".PutTabsBeforeText(tabs));
            CreateClassContent(tabs + 1);
            fileContent.Add("}".PutTabsBeforeText(tabs));
        }

        private void CreateClassContent(int tabs) {
            //Create Properties
            fileContent.Add("private readonly Input input;".PutTabsBeforeText(tabs));
            //Ctor
            fileContent.Add("public ORFScraper(Input input) {".PutTabsBeforeText(tabs));
            fileContent.Add("this.input = input;".PutTabsBeforeText(tabs + 1));
            fileContent.Add("}".PutTabsBeforeText(tabs));
            //Start Method
            fileContent.Add("public void Start() {".PutTabsBeforeText(tabs));
            fileContent.Add("Data data = new Data();".PutTabsBeforeText(tabs + 1));
            CreateUsingForStart(tabs + 1);
            fileContent.Add("}".PutTabsBeforeText(tabs));
            //CreateSteps(tabs);
            CreateMethods(tabs);
        }

        private void CreateMethods(int tabs) {
            foreach (var method in methodsToGenerate) {
                fileContent.Add($"//{method.Signature.Comment}".PutTabsBeforeText(tabs));
                fileContent.Add($"{method.Signature.Accessor} {method.Signature.ReturnType} {method.Signature.Name}()".PutTabsBeforeText(tabs));
                fileContent.Add("{".PutTabsBeforeText(tabs));
                fileContent.AddRange(method.Body.Select(s => s.PutTabsBeforeText(tabs + 1)));
                fileContent.Add("}".PutTabsBeforeText(tabs));
            }
        }

        private void CreateUsingForStart(int tabs) {
            fileContent.Add("using(BasicScraper scraper = new BasicScraper(\"C:\\\\chromedriver2\")) {".PutTabsBeforeText(tabs));
            CreateUsingContentForStart(tabs + 1);
            fileContent.Add("}".PutTabsBeforeText(tabs));
        }

        private void CreateUsingContentForStart(int tabs) {
            //foreach (var method in methodsToGenerate) {
                
            //}
            foreach (var step in input.Steps) {
                if (step.NeedsToIterateOverElements) {
                    DataModel.Action action = step.Actions.First();
                    //fileContent.Add($"{step.Actions.First().PropertyPath} = {step.Name}(scraper);".PutTabsBeforeText(tabs));
                    fileContent.Add($"{action.PropertyPath} = {step.Name}(scraper, () => scraper.FindElements(ByMethod.{action.ElementSelector}, \"{action.ElementIdentifier}\"));".PutTabsBeforeText(tabs));
                }
                else {
                    fileContent.Add($"{step.Name}(scraper);".PutTabsBeforeText(tabs));
                }
            }
        }
    }
   
}
