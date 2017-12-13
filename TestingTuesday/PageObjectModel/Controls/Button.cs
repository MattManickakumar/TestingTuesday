using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace PageObjectModel.Controls
{
    public class Button : ControlBase
    {
        
        public Button(IWebElement webElement) : base(webElement) { }

        public override bool IsClickable
        {
            get
            {
                return _element.Enabled?true:false;
            }
        }
    }
}
