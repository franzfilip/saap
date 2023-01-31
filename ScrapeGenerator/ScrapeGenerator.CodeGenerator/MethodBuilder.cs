using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator {
    //Create a class which is the builder in a builder pattern, it should build MethodToGenerate
    public class MethodBuilder: IMethodBuilder {
        private MethodToGenerate method = new();
        private readonly Step step;
        private readonly Model model;
        
        public MethodBuilder(Step step) {
            this.step = step;
        }

        public void CreateMethodByStep() {
            AddMethodSignature();
        }

        public void AddMethodSignature() {
            method.Signature.Name = step.Name;
            method.Signature.Comment = step.Description;
            method.Signature.ReturnType = GetReturnTypeOfMethod(step.Actions.First());
        }

        private string GetReturnTypeOfMethod(DataModel.Action action, MethodToGenerate method) {
            if (action.Kind == KindOfAction.ITERATE) {
                if (!input.CheckIfTypeIsInModels(action.TypeGenerated)) {
                    throw new ArgumentException($"{action.TypeGenerated} does not exist in the models.");
                }
               return $"List<{action.TypeGenerated}>";
            }
            else {
                return "void";
            }
        }

        public bool CheckIfTypeIsInModels(string type) {
            return model.Models.Any(m => m.ClassName == type);
        }

        public bool CheckIfTypeHasProperty(string property) {
            return Models.Any(m => m.Properties.Any(p => p.Name == property));
        }
    }
}
