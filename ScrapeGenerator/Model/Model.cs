using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    public class Model {
        public string ClassName { get; set; }
        public List<MyProperty> Properties { get; set; }
        public List<Action> Actions { get; set; }
        public bool HasURL { get; set; }
    }
}
