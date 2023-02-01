using System;
using System.Collections.Generic;
using System.Linq;
using DataModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Utility;

namespace CodeGenerator {
    public static class DataClassGenerator {
        public static void GenerateDataClasses(Input input) {
            foreach (var model in input.Models) {
                string classString = $@"
namespace {input.Namespace} {{
    public record {model.ClassName} {{
{String.Join("", CreatePropertyStrings(model.Properties).Select(s => "\t\t" + s))}
    }}
}}
";

                FileOperations.WriteToFile(classString, input.Path, model.ClassName + ".cs");
            }

        }

        private static List<string> CreatePropertyStrings(List<MyProperty> properties) {
            return properties.Select(p => $"public {p.Type} {p.Name} {{ get; set; }}\n").ToList();
        }
    }
}
