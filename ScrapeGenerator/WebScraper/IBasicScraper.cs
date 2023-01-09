﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper {
    public interface IBasicScraper {
        void Navigate(string url);
        public void Read(ByMethod byMethod);
        //public void Iterate();
        //public void Click();
    }
}
