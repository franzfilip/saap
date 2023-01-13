namespace DomainModels {
    public record Input {
        public List<Model> Models { get; set; }
        public List<Step> Steps { get; set; }
    }
}