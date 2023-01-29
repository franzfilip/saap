using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace CodeGenerator {
    public class CodeFactory {
        private readonly Input input;
        private readonly string nameOfScraperObj = "scraper";
        public List<MethodToGenerate> Methods { get; set; }

        public CodeFactory(Input input) {
            this.input = input;
        }

        public void GenerateCode() {
            DataClassGenerator.GenerateDataClasses(input);
            GenerateMethods();
            ScrapeCodeGenerator scrapeCodeGenerator = new ScrapeCodeGenerator(input, Methods);
            scrapeCodeGenerator.CreateFile();
        }

        private void GenerateMethods() {
            Methods = new List<MethodToGenerate>();
            foreach (var step in input.Steps) {
                MethodToGenerate method = new MethodToGenerate();
                method.Signature.Name = step.Name;
                method.Signature.Comment = step.Description;
                SetReturnTypeOfMethod(step.Actions.First(), method);
                CreateMethodBody(step, method);
                Methods.Add(method);
            }
        }

        private void CreateMethodBody(Step step, MethodToGenerate method) {
            foreach (var action in step.Actions) {
                var a = action;
                while (a.SubAction != null) {
                    ActionToGenerate(a, 0, method.Body ??= new List<string>());
                    a = a.SubAction;
                }
            }
        }

        private void SetReturnTypeOfMethod(Model.Action action, MethodToGenerate method) { 
            if (action.Kind == KindOfAction.ITERATE) {
                if (!input.CheckIfTypeIsInModels(action.TypeGenerated)) {
                    throw new ArgumentException($"{action.TypeGenerated} does not exist in the models.");
                }
                method.Signature.ReturnType = $"List<{action.TypeGenerated}>";
            }
            else {
                method.Signature.ReturnType = "void";
            }
        }

        private void ActionToGenerate(Model.Action action, int tabs, List<string> method) {
            switch (action.Kind) {
                case KindOfAction.NAVIGATE:
                    CreateNavigationCall(action, tabs, method);
                    break;
                case KindOfAction.WAITUNTILELEMENTEXISTS:
                    CreateWaitUntilCall(action, tabs, method);
                    break;
                case KindOfAction.CLICK:
                    CreateClickCall(action, tabs, method);
                    break;
                case KindOfAction.ITERATE:
                    CreateIterateCall(action, tabs, method);
                    break;
                case KindOfAction.READ:
                    CreateReadCall(action, tabs, method);
                    break;
            }
        }

        private void CreateNavigationCall(Model.Action action, int tabs, List<string> method) {
            method.Add($"{nameOfScraperObj}.Navigate(\"{action.URL}\");".PutTabsBeforeText(tabs));
        }

        private void CreateWaitUntilCall(Model.Action action, int tabs, List<string> method) {
            method.Add($"{nameOfScraperObj}.WaitUntilElementExists(ByMethod.{action.ElementSelector}, \"{action.ElementIdentifier}\");".PutTabsBeforeText(tabs));
        }

        private void CreateClickCall(Model.Action action, int tabs, List<string> method) {
            if (action.ElementIdentifier == null) {
                method.Add($"{nameOfScraperObj}.TryClickElement(element);".PutTabsBeforeText(tabs));
            }
            else {
                method.Add($"{nameOfScraperObj}.TryClickElement(ByMethod.{action.ElementSelector}, \"{action.ElementIdentifier}\");".PutTabsBeforeText(tabs));
            }
            method.Add($"Thread.Sleep(250);".PutTabsBeforeText(tabs));
        }

        private void CreateIterateCall(Model.Action action, int tabs, List<string> method) {
            method.Add($"{action.PropertyPath} = IterateAndGetMainArticles(scraper, () => {nameOfScraperObj}.FindElements(ByMethod.{action.ElementSelector}, \"{action.ElementIdentifier}\"));".PutTabsBeforeText(tabs));
        }

        private void CreateReadCall(Model.Action action, int tabs, List<string> method) {
            if (action.ElementIdentifier != null) {
                method.Add($"{action.TypeGenerated.ToLower()}.{action.PropertyPath.Split(".")[action.PropertyPath.Split(".").Length - 1]} = {nameOfScraperObj}.Read(ByMethod.{action.ElementSelector}, \"{action.ElementIdentifier}\");".PutTabsBeforeText(tabs));
            }
            else {
                method.Add($"{action.TypeGenerated.ToLower()}.{action.PropertyPath.Split(".")[action.PropertyPath.Split(".").Length - 1]} = {nameOfScraperObj}.Read(element);".PutTabsBeforeText(tabs));
            }
        }


        private void CreateIterateFunction(Step step, int tabs, List<string> method) {
            method.Add($"private List<{step.Actions.First().TypeGenerated}> {step.Name}(BasicScraper {nameOfScraperObj}, Func<List<IWebElement>> getElements) {{".PutTabsBeforeText(tabs));
            CreateIterateFunctionBody(step.Actions, tabs + 1, method);
        }

        private void CreateIterateFunctionBody(List<Model.Action> actions, int tabs, List<string> method, string name = "collected") {
            method.Add("List<IWebElement> elements = getElements();".PutTabsBeforeText(tabs));
            method.Add($"List<{actions.First().TypeGenerated}> {name} = new();".PutTabsBeforeText(tabs));
            method.Add("for(int i = 0; i < elements.Count; i++) {".PutTabsBeforeText(tabs));
            CreateBodyForLoop(actions, tabs + 1, method);
            method.Add("}".PutTabsBeforeText(tabs));
            method.Add($"return {name};".PutTabsBeforeText(tabs));
        }

        private void CreateBodyForLoop(List<Model.Action> actions, int tabs, List<string> method) {
            string typeGenerated = actions.First().TypeGenerated;
            method.Add($"{typeGenerated} {typeGenerated.ToLower()} = new {typeGenerated}();".PutTabsBeforeText(tabs));
            method.Add("IWebElement element = elements[i];".PutTabsBeforeText(tabs));
            CreateSubActions(actions.First(), tabs, method);
            method.Add("{nameOfScraperObj}.NavigateBack();".PutTabsBeforeText(tabs));
            method.Add("elements = getElements();".PutTabsBeforeText(tabs));
            method.Add($"collected.Add({typeGenerated.ToLower()});".PutTabsBeforeText(tabs));
        }

        private void CreateSubActions(Model.Action action, int tabs, List<string> method) {
            action = action.SubAction;
            while (action != null) {
                ActionToGenerate(action, tabs, method);
                action = action.SubAction;
            }
        }
    }
}
