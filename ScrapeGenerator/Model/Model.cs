﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel {
    public record Model {
        public string ClassName { get; set; }
        public List<MyProperty> Properties { get; set; }
        public bool HasURL { get; set; }
    }
}
