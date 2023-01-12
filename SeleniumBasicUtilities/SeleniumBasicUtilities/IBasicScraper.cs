using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumBasicUtilities {
    public interface IBasicScraper {
        void Navigate(string url);
        T Read<T>(ByMethod byMethod, string elementSelector, Action<T> setPropertyAction);
        List<T> ReadMultiple<T>(ByMethod byMethod, string elementSelector, Action<T> setPropertyAction);
        //public void Iterate();
        //public void Click();
    }
}
