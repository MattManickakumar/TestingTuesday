using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PageObjectModel.Controls
{
   public class File: ControlBase
    {   
       public File(IWebElement webElement) : base(webElement) { }

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
