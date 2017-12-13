using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace PageObjectModel.Controls
{
    public class Multiselect : ControlBase
    {
        public Multiselect(IWebElement webElement) : base(webElement) { }

        public override bool IsClickable
        {
            get
            {
                return true;
            }
        }

        public void SelectByValue(IWebDriver driver, string value)
        {
            var elementId = _element.GetAttribute("id");
            ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoMultiSelect').search('{1}');", elementId, value));            
            var selectedoption = driver.FindElement(By.XPath("//*[@id= '"+elementId+"_listbox']//li[text()= '"+value+"']"));   //Sync is place on the driver environment setting.         
            Thread.Sleep(1000);
            selectedoption.Click();
            ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoMultiSelect').trigger('change');", elementId));
            return;
        }

        //issue in multiselect has to modify
        public void SelectByIndex(IWebDriver driver, int value)
        {
            var elementId = _element.GetAttribute("id");            
            ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoMultiSelect').value(['{1}']);", elementId,value));
            return;
        }

        public void SelectByIndex(int index)
        {
            SelectElement Select = new SelectElement(_element);
            Select.SelectByIndex(index);

        }

        public IList<IWebElement> Options
        {
            get
            {

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
                return new SelectElement(_element);
            }
        }

    }
}
