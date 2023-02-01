namespace DataModel {
    public record Input {
        public string Namespace { get; set; }
        public List<Model> Models { get; set; }
        public List<Step> Steps { get; set; }
        public string Path { get; set; }

        public bool CheckIfTypeIsInModels(string type) {
            return Models.Any(m => m.ClassName == type);
        }

        public bool CheckIfTypeHasProperty(string property) {
            return Models.Any(m => m.Properties.Any(p => p.Name == property));
        }
    }
}