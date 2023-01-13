using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModels {
    public class Model {
        public string ClassName { get; set; }
        public List<(string Type, string Name)> Properties { get; set; }
        public bool HasURL { get; set; }
    }
}
