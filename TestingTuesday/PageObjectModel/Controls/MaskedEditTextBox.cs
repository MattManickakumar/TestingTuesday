using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace PageObjectModel.Controls
{
    public class MaskedEditTextBox : ControlBase
    {        
        protected IWebDriver _driver;
        public MaskedEditTextBox(IWebElement webElement) : base(webElement) { }
        public MaskedEditTextBox(IWebElement webElement, IWebDriver driver) : base(webElement) {
            _driver = driver;
        }

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

        public override void Type(string value)
        {
            var elementId = _element.GetAttribute("id");
            ((IJavaScriptExecutor)_driver).ExecuteScript(String.Format("$('#{0}').data('kendoMaskedTextBox').value('{1}');", elementId, value));
            ((IJavaScriptExecutor)_driver).ExecuteScript(String.Format("$('#{0}').data('kendoMaskedTextBox').trigger('change');", elementId));
        }
        public void SetValue(IWebDriver driver, string value)
        {
            var elementId = _element.GetAttribute("id");
            ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoMaskedTextBox').value({1});", elementId, value));
            ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoMaskedTextBox').trigger('change');", elementId));
        }
    }
}
