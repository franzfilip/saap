using DomainModels;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumBasicUtilities {
    public interface IBasicScraper {
        void Navigate(string url);
        bool WaitUntilElementExists(ByMethod waitUntilElementBy, string waitUntilElementExists);
        bool TryClickElement(ByMethod byMethod, string elementSelector);
        IWebElement FindElement(ByMethod by, string name);
        List<IWebElement> FindElements(ByMethod by, string name);
        IWebElement FindElement(ByMethod by, string name, IWebElement element);
        List<IWebElement> FindElements(ByMethod by, string name, IWebElement element);
        void NavigateBack();
        bool TryClickElement(IWebElement element);
        T Read<T>(ByMethod by, string elementSelector, Action<T, string> setPropertyAction);
        T Read<T>(IWebElement element, Action<T, string> setPropertyAction);
        //public void Iterate();
        //public void Click();
    }
}
