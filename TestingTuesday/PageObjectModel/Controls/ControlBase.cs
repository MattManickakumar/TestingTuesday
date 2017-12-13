using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PageObjectModel.Controls
{
    public abstract class ControlBase:IWebElement
    {
       protected IWebElement _element;

       public ControlBase(IWebElement webElement){
           _element = webElement;            
       }

       public bool Displayed
       {
           get { return _element.Displayed; }
       }

       public bool Enabled
       {
           get { return _element.Enabled; }
       }

       public string GetAttribute(string attributeName)
       {
           return _element.GetAttribute(attributeName);
       }

       public string GetCssValue(string propertyName)
       {
           return _element.GetCssValue(propertyName);
       }

       public string GetProperty(string propertyName)
       {
            return _element.GetProperty(propertyName);
       }
       public Point Location
       {
           get { return _element.Location; }
       }

       public bool Selected
       {
           get { return _element.Selected; }
       }

       public void SendKeys(string text)
       {
           _element.SendKeys(text);
       }

       public Size Size
       {
           get { return _element.Size; }
       }

       public void Submit()
       {
           _element.Submit();
       }

       public string TagName
       {
           get { return _element.TagName; }
       }

       public IWebElement FindElement(By by)
       {
           return _element.FindElement(by);
       }

       public System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> FindElements(By by)
       {
           return _element.FindElements(by);
       }

       public string Text
       {
           get
           {
               return _element.Text;
           }
       }

       public virtual void Type(string text)
       {
            if (IsTypeable && Displayed)
                _element.Click();
               _element.SendKeys(text);
        }

        public void Click()
       {
        
            if (IsClickable && Displayed)
               _element.Click();
       }

       public virtual bool IsClickable
       {
           get
           {
                return false;
           }
       }

       public virtual bool IsTypeable
       {
           get
           {
               return false;
           }
       }

       public void Clear()
       {
           _element.Clear();
       }

       public string Value
       {
           get
           {
               return _element.GetAttribute("value");
           }
       }
        public bool IsInvalid
        {
            get
            {
                return _element.GetAttribute("aria-invalid").ToLower()=="true"?true:false;
            }
        }

    }
}
