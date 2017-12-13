using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace PageObjectModel.Controls
{
    public class DateTimePicker : ControlBase
    {
        
        public DateTimePicker(IWebElement webElement) : base(webElement) { }

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

        public void SetValue(IWebDriver driver, DateTime date)
        {
            var elementId = _element.GetAttribute("id");
            ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoDatePicker').value(new Date({1},{2},{3}));", elementId,date.Year,date.Month,date.Day));
            ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoDatePicker').trigger('change');", elementId));
        }

        public void SetValue(IWebDriver driver, string date)
        {
            var elementId = _element.GetAttribute("id");
            ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoDatePicker').value('{1}');", elementId, date));
            ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoDatePicker').trigger('change');", elementId));
        }

    }
}
