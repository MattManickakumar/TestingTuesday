using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PageObjectModel.Controls
{
    public class TextBox : ControlBase
    {
        //private IWebElement element;

        public TextBox(IWebElement webElement) : base(webElement) { }

        public override bool IsTypeable
        {
            get
            {
                return true;
            }
        }

        public override bool IsClickable
        {
            get
            {
                return true;
            }
        }

    }
}
