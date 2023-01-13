using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModels {
    public class Step {
        public List<Action> Actions { get; set; }
        public string Description { get; set; }
    }
}
