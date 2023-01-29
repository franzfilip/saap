using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    public record Step {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool NeedsToIterateOverElements { get; set; }
        public List<Action> Actions { get; set; }
        public bool SpecialImplNeeded() {
            return Actions.Any(a => a.Kind == KindOfAction.ITERATE);
        }
    }
}
