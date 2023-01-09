using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ScrapeGenerator.CodeGenerator {
    public static class DataClassGenerator {
        public static string GenerateDataClass(Input input) {
            // Create a list to hold the property strings
            List<string> propertyStrings = CreatePropertyStrings(input.Properties);

            // Join the property strings into a single string
            string propertiesString = string.Join("\n", propertyStrings);

            // Create the class string
            string classString = $@"
            public class {input.ClassName}
            {{
                {propertiesString}
            }}
        ";

            return classString;
        }

        private static List<string> CreatePropertyStrings(List<(string Type, string Name)> properties) {
            return properties.Select(p => $"public {p.Type} {p.Name} {{ get; set; }}").ToList();
        }
    }
}
