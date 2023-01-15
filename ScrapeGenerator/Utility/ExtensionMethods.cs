using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility {
    public static class ExtensionMethods {
        public static string PutTabsBeforeText(this string text, int amount) {
            return String.Concat(Enumerable.Repeat("\t", amount)) + text;
        }
    }
}
