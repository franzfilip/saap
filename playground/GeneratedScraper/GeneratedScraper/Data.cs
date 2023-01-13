using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratedScraper {
    public record Data: IHasURL {
        public List<Article> MainArticles { get; set; }
        public List<Article> SubArticles { get; set; }
        public string URL { get; set; }
    }
}
