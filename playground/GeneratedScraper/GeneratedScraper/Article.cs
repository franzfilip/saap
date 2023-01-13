﻿using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratedScraper {
    public record Article: IHasURL {
        public string Text { get; set; }
        public string URL { get; set; }
    }
}
