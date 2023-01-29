using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator {
    public record MethodSignature {
        public string Comment { get; set; }
        public string Accessor { get; set; } = "private";
        public string ReturnType { get; set; }
        //public Type ReturnType { get; set; }
        public string Name { get; set; }
        public List<Tuple<Type, string>> Param { get; set; } = new();
    }
}
