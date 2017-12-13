using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using System.Threading;

namespace PageObjectModel.Controls
{
    public class Accordion:ControlBase
    {
        //private IWebElement element;

        public Accordion(IWebElement webElement) : base(webElement) { }

        public override bool IsClickable
        {
            get
            {
                return true;
            }
        }

        public bool IsExpanded {
            get
            {
                Thread.Sleep(2000); //Wait for 2 seconds before evaluation.
                return _element.GetAttribute("aria-expanded") == "true" ? true : false;
            }
        }

    }
}
