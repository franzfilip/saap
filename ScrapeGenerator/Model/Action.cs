using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    public record Action {
        public KindOfAction Kind { get; set; }
        public string URL { get; set; }
        public string PropertyPath { get; set; }
        public ByMethod ElementSelector { get; set; }
        public string ElementIdentifier { get; set; }
        public Action SubAction { get; set; }
        public string TypeGenerated { get; set; }

        public bool GeneratesData() {
            return Kind == KindOfAction.READ || Kind == KindOfAction.ITERATE;
        }
    }
}
