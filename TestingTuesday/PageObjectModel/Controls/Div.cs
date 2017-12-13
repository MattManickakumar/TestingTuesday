using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
//using OpenQA.Selenium.Interactions;

namespace PageObjectModel.Controls
{
    public class Div:ControlBase
    {
        
        public Div(IWebElement webElement) : base(webElement) { }

        public override bool IsClickable
        {
            get
            {
                return true;
            }
        }

        public bool AreaExpanded
        {
            get
            {
                var _parentNode = _element.FindElement(By.XPath("./../.."));                
                string result = _parentNode.GetAttribute("aria-expanded");

                if (result == "true") return true;
                else return false;
            }
        }
        public bool IsExpanded
        {
            get
            {                
                return _element.GetAttribute("aria-expanded") == "true" ? true : false;
            }
        }

    }
}
