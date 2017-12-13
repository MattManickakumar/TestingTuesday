using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace PageObjectModel.Controls
{
    public class Tab : ControlBase
    {
        //private IWebElement element;

        public Tab(IWebElement webElement) : base(webElement) { }

        public override bool IsClickable
        {
            get
            {
                return true;
            }
        }

        public string Title {
            get {
                return _element.GetAttribute("title");
            }
        }
    }
}
