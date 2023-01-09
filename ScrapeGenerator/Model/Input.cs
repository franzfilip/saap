namespace Model {
    public record Input {
        public string ClassName { get; set; }
        public List<(string Type, string Name)> Properties { get; set; }
        public List<Action> Actions { get; set; }
    }
}