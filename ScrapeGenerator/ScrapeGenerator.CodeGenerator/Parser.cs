using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator {
    public class Parser {
        private readonly Input input;

        public Parser(Input input) {
            this.input = input;
        }

        public bool AssertInputCorrect() {

            return true;
        }

        //public bool CheckIfTypeToBeGeneratedByActionsIsInModel() {
        //    foreach(var step in input.Steps) {
        //        foreach (var action in step.Actions) {
        //            var a = action;
        //            while (a.SubAction != null) {
        //                if(a.TypeGenerated != null) {
        //                    if (!CheckIfTypeIsInModels(a.TypeGenerated)) {
        //                        return false;
        //                    }
        //                }
        //                a = a.SubAction;
        //            }
        //        }
        //    }

        //    return true;
        //}
        public bool IsTypeGeneratedByActionsInModel() {
            return input.Steps
                .SelectMany(step => step.Actions)
                .Where(action => action.TypeGenerated != null)
                .Select(action => GetSubActions(action))
                .SelectMany(actions => actions)
                .All(typeGenerated => CheckIfTypeIsInModels(typeGenerated));
        }

        private IEnumerable<string> GetSubActions(DataModel.Action action) {
            while (action.SubAction != null) {
                if (action.TypeGenerated != null) {
                    yield return action.TypeGenerated;
                }
                action = action.SubAction;
            }
        }

        public bool CheckIfTypeIsInModels(string type) {
            return input.Models.Any(m => m.ClassName.Equals(type));
        }

    }
}
