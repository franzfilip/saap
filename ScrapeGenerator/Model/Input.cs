namespace Model {
    public record Input {
        public string Namespace { get; set; }
        public List<Model> Models { get; set; }
        public string Path { get; set; }
    }
}