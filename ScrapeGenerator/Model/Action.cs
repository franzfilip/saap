﻿using System;
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
        public Action SubAction { get; set; }
    }
}
