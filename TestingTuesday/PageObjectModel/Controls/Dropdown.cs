using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PageObjectModel.Controls
{
    public class Dropdown:ControlBase
    {
        public Dropdown(IWebElement webElement) : base(webElement) { }

        public override bool IsClickable
        {
            get
            {
                return true;
            }
        }


        //to be used for all kendo dropdowns.
        public void SelectByValue(IWebDriver driver, string value) {
            
            var elementIndex = 0;
            var elementId = _element.GetAttribute("id");
            var options = _element.FindElements(By.TagName("option"));

                    
            foreach( var element in options)
            {
                var optionText = element.GetAttribute("text");
                if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(optionText) && optionText.Trim().ToLower() == value.Trim().ToLower())
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoDropDownList').select({1});", elementId, elementIndex));
                    ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoDropDownList').trigger('change');", elementId));
                    return;
                }
                elementIndex += 1;
            };

        }


        //to be used for kendo dropdowns controls present within Grid. Eg: dropdown list within a grid cell.
        public void WGrid_SelectByValue(IWebDriver driver, string value)
        {

            var elementIndex = 1;
            var elementId = _element.GetAttribute("id");
            var options = driver.FindElements(By.XPath(String.Format("//*[@id='{0}_listbox']//li",elementId)));

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

        public void WGrid_SelectByValue(IWebDriver driver, int index)
        {
           
            var elementId = _element.GetAttribute("id");
            
            ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoDropDownList').select({1});", elementId, index));
            ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoDropDownList').trigger('change');", elementId));
            return; 
                           
        }

        
        public void WGrid_SelectByValue(IWebDriver driver, string value, bool SearchBy)
        {
            // if SearchBy = True, then selects an option equal to the value given
            //    Search = False, vice versa
            if (!SearchBy)
            {
                           

                    var elementIndex = 1;
                    var elementId = _element.GetAttribute("id");
                    var options = driver.FindElements(By.XPath(String.Format("//*[@id='{0}_listbox']//li", elementId)));

                    //options.ToList().ForEach(element =>
                    foreach( var element in options)
                    {
                        var optionText = element.GetAttribute("textContent");
                        if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(optionText) && optionText.Trim() != value.Trim())
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoDropDownList').select({1});", elementId, elementIndex));
                            ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoDropDownList').trigger('change');", elementId));
                            break;                            
                        }
                        elementIndex += 1;
                    };

            } else
            {
                WGrid_SelectByValue(driver, value);
            }

        }


        public void SelectByIndex(int index)
        {
            SelectElement Select = new SelectElement(_element);
            Select.SelectByIndex(index);           

        }

        //Could be used only for non-kendo drop downs.
        public void SelectByText(string Text)
        {
            SelectElement Select = new SelectElement(_element);
            Select.SelectByText(Text);

        }

        public IList<IWebElement> Options
        {
            get{

                SelectElement Select = new SelectElement(_element);
                return Select.Options;
            }
        }

        public IWebElement SelectedOption
        {
            get
            {
                SelectElement Select = new SelectElement(_element);
                return Select.SelectedOption;
            }
        }

        public SelectElement SelectElement
        {
            get
            {
                return  new SelectElement(_element);
            }
        }
        public String SelectedOptionText
        {
            //returns the selected option in a kendoDropdown
            get
            {
                string value = _element.GetAttribute("value");
                IWebElement selectedOption = _element.FindElement(By.XPath(".//option[@value='" + value + "']"));
                return selectedOption.GetAttribute("textContent");
            }
        }
    }
}
