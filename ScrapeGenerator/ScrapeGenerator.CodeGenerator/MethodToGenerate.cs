using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator {
    public record MethodToGenerate {
        public MethodSignature Signature { get; set; } = new();
        public List<string> Body { get; set; } = new();
    }
}
