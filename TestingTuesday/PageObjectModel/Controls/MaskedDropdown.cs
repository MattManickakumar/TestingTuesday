using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PageObjectModel.Controls
{
    public class MaskedDropdown : ControlBase
    {
        
        public MaskedDropdown(IWebElement webElement) : base(webElement) { }

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

        public void SelectByValue(IWebDriver driver, string value)
        {

            var elementIndex = 1;
            var elementId = _element.GetAttribute("id");
            var options = driver.FindElements(By.XPath(String.Format("//*[@id='{0}_listbox']//li", elementId)));

            options.ToList().ForEach(element =>
            {
                var optionText = element.GetAttribute("textContent");
                if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(optionText) && optionText.Trim() == value.Trim())
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoDropDownList').select({1});", elementId, elementIndex));
                    ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoDropDownList').trigger('change');", elementId));
                    return;
                }
                elementIndex += 1;
            });

        }

        public void SelectByValue(IWebDriver driver, int index)
        {

            var elementId = _element.GetAttribute("id");

            ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoDropDownList').select({1});", elementId, index));
            ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoDropDownList').trigger('change');", elementId));
            return;

        }

        public IList<IWebElement> Options(IWebDriver driver)
        {
            
            {
                
                var elementId = _element.GetAttribute("id");
                var options = driver.FindElements(By.XPath(String.Format("//*[@id='{0}_listbox']//li", elementId)));
                return options;
            }
        }
    }
}
