using Model;
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
            foreach(var method in methodsToGenerate) {
                fileContent.Add($"//{method.Signature.Comment}".PutTabsBeforeText(tabs));
                fileContent.Add($"{method.Signature.Accessor} {method.Signature.ReturnType} {method.Signature.Name}()".PutTabsBeforeText(tabs));
                fileContent.Add("{".PutTabsBeforeText(tabs));
                fileContent.AddRange(method.Body.Select(s => s.PutTabsBeforeText(tabs + 1)));
                fileContent.Add("}".PutTabsBeforeText(tabs));
            }
        }

        private void CreateUsingForStart(int tabs) {
            fileContent.Add("using(BasicScraper scraper = new BasicScraper(\"C:\\\\chromedriver2\")) {".PutTabsBeforeText(tabs));
            //CreateUsingContentForStart(tabs + 1);
            fileContent.Add("}".PutTabsBeforeText(tabs));
        }

        private void CreateUsingContentForStart(int tabs) {
            foreach (var step in input.Steps) {
                if (step.NeedsToIterateOverElements) {
                    Model.Action action = step.Actions.First();
                    //fileContent.Add($"{step.Actions.First().PropertyPath} = {step.Name}(scraper);".PutTabsBeforeText(tabs));
                    fileContent.Add($"{action.PropertyPath} = {step.Name}(scraper, () => scraper.FindElements(ByMethod.{action.ElementSelector}, \"{action.ElementIdentifier}\"));".PutTabsBeforeText(tabs));
                }
                else {
                    fileContent.Add($"{step.Name}(scraper);".PutTabsBeforeText(tabs));
                }
            }
        }

        private void CreateSteps(int tabs) {
            foreach(var step in input.Steps) {
                CreateStep(step, tabs);
            }
        }

        private void CreateStep(Step step, int tabs) {
            fileContent.Add($"//{step.Description}".PutTabsBeforeText(tabs));
            fileContent.Add($"private void {step.Name} (BasicScraper scraper){{".PutTabsBeforeText(tabs));
            CreateStepContent(step, tabs + 1);
            fileContent.Add("}".PutTabsBeforeText(tabs));
        }

        private void CreateStepContent(Step step, int tabs) {
            foreach(var action in step.Actions) {
                if (step.NeedsToIterateOverElements) {
                    fileContent.Remove(fileContent.Last());
                    CreateIterateFunction(step, tabs - 1);
                }
                else {
                    CreateActionAndSubActions(action, tabs);
                }
            }
        }

        private void CreateActionAndSubActions(Model.Action action, int tabs) {
            ActionToGenerate(action, tabs);
            action = action.SubAction;
            while(action != null) {
                ActionToGenerate(action, tabs);
                action = action.SubAction;
            }
        }

        private void CreateSubActions(Model.Action action, int tabs) {
            action = action.SubAction;
            while (action != null) {
                ActionToGenerate(action, tabs);
                action = action.SubAction;
            }
        }

        private void ActionToGenerate(Model.Action action, int tabs) {
            switch (action.Kind) {
                case KindOfAction.NAVIGATE:
                    CreateNavigationCall(action, tabs);
                    break;
                case KindOfAction.WAITUNTILELEMENTEXISTS:
                    CreateWaitUntilCall(action, tabs);
                    break;
                case KindOfAction.CLICK:
                    CreateClickCall(action, tabs);
                    break;
                case KindOfAction.ITERATE:
                    CreateIterateCall(action, tabs);
                    break;
                case KindOfAction.READ:
                    CreateReadCall(action, tabs);
                    break;
            }
        }

        private void CreateNavigationCall(Model.Action action, int tabs) {
            fileContent.Add($"scraper.Navigate(\"{action.URL}\");".PutTabsBeforeText(tabs));
        }

        private void CreateWaitUntilCall(Model.Action action, int tabs) {
            fileContent.Add($"scraper.WaitUntilElementExists(ByMethod.{action.ElementSelector}, \"{action.ElementIdentifier}\");".PutTabsBeforeText(tabs));
        }

        private void CreateClickCall(Model.Action action, int tabs) {
            if(action.ElementIdentifier == null) {
                fileContent.Add($"scraper.TryClickElement(element);".PutTabsBeforeText(tabs));
            }
            else {
                fileContent.Add($"scraper.TryClickElement(ByMethod.{action.ElementSelector}, \"{action.ElementIdentifier}\");".PutTabsBeforeText(tabs));
            }
            fileContent.Add($"Thread.Sleep(250);".PutTabsBeforeText(tabs));
        }

        private void CreateIterateCall(Model.Action action, int tabs) {
            fileContent.Add($"{action.PropertyPath} = IterateAndGetMainArticles(scraper, () => scraper.FindElements(ByMethod.{action.ElementSelector}, \"{action.ElementIdentifier}\"));".PutTabsBeforeText(tabs));
        }

        private void CreateReadCall(Model.Action action, int tabs) {
            if(action.ElementIdentifier != null) {
                fileContent.Add($"{action.TypeGenerated.ToLower()}.{action.PropertyPath.Split(".")[action.PropertyPath.Split(".").Length - 1]} = scraper.Read(ByMethod.{action.ElementSelector}, \"{action.ElementIdentifier}\");".PutTabsBeforeText(tabs));
            }
            else {
                fileContent.Add($"{action.TypeGenerated.ToLower()}.{action.PropertyPath.Split(".")[action.PropertyPath.Split(".").Length - 1]} = scraper.Read(element);".PutTabsBeforeText(tabs));
            }
        }


        private void CreateIterateFunction(Step step, int tabs) {
            fileContent.Add($"private List<{step.Actions.First().TypeGenerated}> {step.Name}(BasicScraper scraper, Func<List<IWebElement>> getElements) {{".PutTabsBeforeText(tabs));
            CreateIterateFunctionBody(step.Actions, tabs + 1);
        }

        private void CreateIterateFunctionBody(List<Model.Action> actions, int tabs, string name = "collected") {
            fileContent.Add("List<IWebElement> elements = getElements();".PutTabsBeforeText(tabs));
            fileContent.Add($"List<{actions.First().TypeGenerated}> {name} = new();".PutTabsBeforeText(tabs));
            fileContent.Add("for(int i = 0; i < elements.Count; i++) {".PutTabsBeforeText(tabs));
            CreateBodyForLoop(actions, tabs + 1);
            fileContent.Add("}".PutTabsBeforeText(tabs));
            fileContent.Add($"return {name};".PutTabsBeforeText(tabs));
        }

        private void CreateBodyForLoop(List<Model.Action> actions, int tabs) {
            string typeGenerated = actions.First().TypeGenerated;
            fileContent.Add($"{typeGenerated} {typeGenerated.ToLower()} = new {typeGenerated}();".PutTabsBeforeText(tabs));
            fileContent.Add("IWebElement element = elements[i];".PutTabsBeforeText(tabs));
            CreateSubActions(actions.First(), tabs);
            fileContent.Add("scraper.NavigateBack();".PutTabsBeforeText(tabs));
            fileContent.Add("elements = getElements();".PutTabsBeforeText(tabs));
            fileContent.Add($"collected.Add({typeGenerated.ToLower()});".PutTabsBeforeText(tabs));
        }
    }
}
