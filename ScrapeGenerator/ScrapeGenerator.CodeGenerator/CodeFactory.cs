using DataModel;
using System;
using System.Reflection.Metadata;
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
                GenerateMethod(step);
            }
        }

        private void GenerateMethod(Step step) {
            MethodToGenerate method = new MethodToGenerate();
            method.Signature.Name = step.Name;
            method.Signature.Comment = step.Description;
            SetReturnTypeOfMethod(step.Actions.First(), method);
            CreateMethodBody(step, method);
            Methods.Add(method);
        }

        private void CreateMethodBody(Step step, MethodToGenerate method) {
            foreach (var action in step.Actions) {
                var a = action;
                while (a != null) {
                    if(a.Kind == KindOfAction.ITERATE) {
                        CreateIterateFunction(step, 0, method.Body ??= new List<string>());
                        SetParamsForMethod(method, action, true);
                        return;
                    }
                    else {
                        ActionToGenerate(a, 0, method.Body ??= new List<string>());
                    }
                    a = a.SubAction;
                }
            }
        }

        private void SetParamsForMethod(MethodToGenerate method, DataModel.Action action, bool reloadsElements) {
            method.Signature.Params ??= new List<string>();
            method.Signature.Params.Add("IBasicScraper scraper");
            if (reloadsElements) {
                string getElements = $"() => scraper.FindElements(ByMethod.{action.ElementSelector}, \"{action.ElementIdentifier}\");";
                method.Signature.Params.Add($"Func<List<IWebElement>> getElements = {getElements}" );
            }
        }
        private void SetReturnTypeOfMethod(DataModel.Action action, MethodToGenerate method) { 
            if (action.Kind == KindOfAction.ITERATE) {
                if (!input.CheckIfTypeIsInModels(action.TypeGenerated)) {
                    throw new ArgumentException($"{action.TypeGenerated} does not exist in the models.");
                }
                method.Signature.ReturnType = $"List<{action.TypeGenerated}>";
            }
            else if(action.Kind == KindOfAction.READ) {
                method.Signature.ReturnType = action.TypeGenerated;
            }
            else {
                method.Signature.ReturnType = "void";
            }
        }

        private void ActionToGenerate(DataModel.Action action, int tabs, List<string> method, bool isSubAction = false) {
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
               case KindOfAction.READ:
                    CreateReadCall(action, tabs, method);
                    break;
                default:
                    if (action.Kind == KindOfAction.ITERATE)
                        throw new ArgumentException();

                    throw new NotSupportedException();
            }
        }

        private void CreateNavigationCall(DataModel.Action action, int tabs, List<string> method) {
            method.Add($"{nameOfScraperObj}.Navigate(\"{action.URL}\");".PutTabsBeforeText(tabs));
        }

        private void CreateWaitUntilCall(DataModel.Action action, int tabs, List<string> method) {
            method.Add($"{nameOfScraperObj}.WaitUntilElementExists(ByMethod.{action.ElementSelector}, \"{action.ElementIdentifier}\");".PutTabsBeforeText(tabs));
        }

        private void CreateClickCall(DataModel.Action action, int tabs, List<string> method) {
            if (action.ElementIdentifier == null) {
                method.Add($"{nameOfScraperObj}.TryClickElement(element);".PutTabsBeforeText(tabs));
            }
            else {
                method.Add($"{nameOfScraperObj}.TryClickElement(ByMethod.{action.ElementSelector}, \"{action.ElementIdentifier}\");".PutTabsBeforeText(tabs));
            }
            method.Add($"Thread.Sleep(250);".PutTabsBeforeText(tabs));
        }

        private void CreateReadCall(DataModel.Action action, int tabs, List<string> method) {
            if (action.ElementIdentifier != null) {
                method.Add($"{action.TypeGenerated.ToLower()}.{action.PropertyPath.Split(".")[action.PropertyPath.Split(".").Length - 1]} = {nameOfScraperObj}.Read(ByMethod.{action.ElementSelector}, \"{action.ElementIdentifier}\");".PutTabsBeforeText(tabs));
            }
            else {
                method.Add($"{action.TypeGenerated.ToLower()}.{action.PropertyPath.Split(".")[action.PropertyPath.Split(".").Length - 1]} = {nameOfScraperObj}.Read(element);".PutTabsBeforeText(tabs));
            }
        }

        private void CreateIterateFunction(Step step, int tabs, List<string> methodBody) {
            CreateIterateFunctionBody(step.Actions, tabs, methodBody);
        }

        private void CreateIterateFunctionBody(List<DataModel.Action> actions, int tabs, List<string> methodBody, string name = "collected") {
            methodBody.Add("List<IWebElement> elements = getElements();".PutTabsBeforeText(tabs));
            methodBody.Add($"List<{actions.First().TypeGenerated}> {name} = new();".PutTabsBeforeText(tabs));
            methodBody.Add("for(int i = 0; i < elements.Count; i++) {".PutTabsBeforeText(tabs));
            CreateBodyForLoop(actions, tabs + 1, methodBody);
            methodBody.Add("}".PutTabsBeforeText(tabs));
            methodBody.Add($"return {name};".PutTabsBeforeText(tabs));
        }

        private void CreateBodyForLoop(List<DataModel.Action> actions, int tabs, List<string> methodBody) {
            string typeGenerated = actions.First().TypeGenerated;
            methodBody.Add($"{typeGenerated} {typeGenerated.ToLower()} = new {typeGenerated}();".PutTabsBeforeText(tabs));
            methodBody.Add("IWebElement element = elements[i];".PutTabsBeforeText(tabs));
            CreateSubActions(actions.First(), tabs, methodBody);
            methodBody.Add("scraper.NavigateBack();".PutTabsBeforeText(tabs));
            methodBody.Add("elements = getElements();".PutTabsBeforeText(tabs));
            methodBody.Add($"collected.Add({typeGenerated.ToLower()});".PutTabsBeforeText(tabs));
        }

        private void CreateSubActions(DataModel.Action action, int tabs, List<string> methodBody) {
            action = action.SubAction;
            while (action != null) {
                ActionToGenerate(action, tabs, methodBody);
                action = action.SubAction;
            }
        }
    }
}
