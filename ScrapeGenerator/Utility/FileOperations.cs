using Model;
using Newtonsoft.Json;

namespace Utility {
    public static class FileOperations {
        public static Input ReadJson(string filePath) {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Input>(json);
        }

        public static 
            void WriteToFile(string content, string filePath, string fileName) {
            string fullPath = Path.Combine(filePath, fileName);
            File.WriteAllText(fullPath, content);
        }
    }
}