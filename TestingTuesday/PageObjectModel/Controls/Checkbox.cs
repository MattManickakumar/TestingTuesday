using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace PageObjectModel.Controls
{
    public class Checkbox : ControlBase
    {
        
        public Checkbox(IWebElement webElement) : base(webElement) { }

        public override bool IsTypeable
        {
            get
            {
                return false;
            }
        }

        public override bool IsClickable
        {
            get
            {
                return true;
            }
        }

        public bool Checked 
        {
            get 
            {
                return _element.Selected;
            }
        }
    }
}
