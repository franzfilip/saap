using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator {
    public class MethodSignature {
        public string Accessor { get; set; } = "private";
        public Type ReturnType { get; set; }
        public string Name { get; set; }
        public List<Tuple<Type, string>> Param { get; set; }
    }
}
