using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace PageObjectModel.Controls
{
    public class Form : ControlBase
    {
        
        public Form(IWebElement webElement) : base(webElement) { }

        public override bool IsClickable
        {
            get
            {
                return true;
            }
        }

    }
}
