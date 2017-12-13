using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Internal;
using PageObjectModel.Controls;

namespace Praksys.PageObjects.Core
{
    public abstract class PageBase: ISearchContext
    {
        protected int _implicitWait = 1;

        protected IWebDriver _driver;
        protected bool _loaded = false;
        protected string _scopeid = "";

        public PageBase(IWebDriver driver) {
            _driver = driver;
            Thread.Sleep(_implicitWait * 1000);
           // Sync();
        }

        public bool IsLoaded { get { return _loaded; } }

        public virtual void WaitUntilPageLoad() { }

        public string WrapScopeID(string elementid)
        {
            return string.Format("{0}{1}", elementid, _scopeid);
        }

        public string GetAlertText
        { 
            get
            {
                return _driver.SwitchTo().Alert().Text;
            }
        }

        public IAlert Alert
        {
            get 
            {
                return _driver.SwitchTo().Alert();
            }
        }

        public Div ToastContainer
        { 
            get
            {
                WebDriverWait wait = new WebDriverWait(_driver, new TimeSpan(0, _implicitWait, 0));
                return new Div(wait.Until(ExpectedConditions.ElementIsVisible(By.Id("toast-container"))));
            }
        }

        public string ToastMessageTitle 
        {
            get 
            {
                return ToastContainer.FindElement(By.ClassName("toast-title")).Text;
            }
        }
        
        public void SelfServiceSync()
        {

            /*
             * Synchronize the automation run for the general data presenatation praksys takes to display in grid, frame, tabs etc 
             */

            Thread.Sleep(2000);

            WebDriverWait wait = new WebDriverWait(_driver, new TimeSpan(0, _implicitWait, 0));
            IWebElement element = wait.Until(driver => driver.FindElement((By.XPath("//*[contains(@id,'dvSpinner')]"))));   // Spinner  : selfservice
            SpinnerHandler(element);


        }

        public void Sync()
        {

            /*
             * Synchronize the automation run for the general data presenatation praksys takes to display in grid, frame, tabs etc 
             */

            Thread.Sleep(2000);     

            WebDriverWait wait = new WebDriverWait(_driver, new TimeSpan(0, 2, 0));
            IWebElement element = wait.Until(driver => driver.FindElement((By.XPath("//*[@id='dvLoading']"))));   // Spinner 1 : Production environment
            SpinnerHandler(element);

            IWebElement element1 = wait.Until(driver => driver.FindElement((By.XPath("//*[@id='dvSpinner']"))));   // Spinner 2 : EDU
            SpinnerHandler(element1);

            IWebElement element2 = wait.Until(driver => driver.FindElement((By.XPath("//*[@id='dvLoadingUA']")))); // Spinner 3: QA
            SpinnerHandler(element2);
            
        }

        private bool SpinnerHandler(IWebElement element)
        {
            bool Flag = false;
            bool SpinnerAppeared = false;
            do
            {
                if (element.GetAttribute("style").Equals("display: block;"))
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("I'm waiting: ... Sync");
                    SpinnerAppeared = true;

                }
                else Flag = true;
            } while (!Flag);

            return SpinnerAppeared;            
        }

        public bool IsSpinnerPresent()
        {
            bool result = false;

            Thread.Sleep(2000);
            
            IWebElement element = _driver.FindElement((By.XPath("//*[@id='dvLoading']")));   // Spinner 1 : Production environment
            if(SpinnerHandler(element)) return true;

            IWebElement element1 = _driver.FindElement((By.XPath("//*[@id='dvSpinner']")));   // Spinner 2 : EDU
            if (SpinnerHandler(element1)) return true;

            IWebElement element2 = _driver.FindElement((By.XPath("//*[@id='dvLoadingUA']"))); // Spinner 3: QA
            if (SpinnerHandler(element2)) return true;

            return result;

        }

        public void Wait(String Element_Path)
        {
            WebDriverWait wait = new WebDriverWait(_driver, new TimeSpan(0, _implicitWait, 0));
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(Element_Path)));

        }

        public IWebElement Sync(String Element_Path)
        {
            /*
             * Synchronize the automation run based on the visiblity of the object.
             * Note : This method is designed to be called from all classes within 'PageObjects' namespace.
             */
            
            WebDriverWait wait = new WebDriverWait(_driver, new TimeSpan(0, _implicitWait, 0));
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(Element_Path)));
            return element;
        }

        public IWebElement Sync(By by)
        {
            /*
             * Synchronize the automation run based on the visiblity of the object.
             * Note : This method is designed to be called from all classes within 'PageObjects' namespace.
             */            
            WebDriverWait wait = new WebDriverWait(_driver, new TimeSpan(0, _implicitWait, 0));
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(by));
            return element;
        }

        public string GetSessionID_ActiveTab(string PartialID)
        {
            /*
             * Return the session ID of the active tab.
             */
            var session = _driver.FindElement(By.XPath("//*[@id='tabController']/div/div/div[contains(@class,'active')]//div[contains(@id,'" + PartialID.Trim() + "')]"));

            return session.GetAttribute("id").Replace(PartialID, "").Trim();
        }

        public IWebElement FindElement(By by)
        {
            IWebElement element;

            WebDriverWait wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 15));                
            element = wait.Until((driver => _driver.FindElement(by)));
            //Console.WriteLine("1. PageBase.FindElement: " + by.ToString());
            return element;            
            
        }

        public ReadOnlyCollection <IWebElement> FindElements(By by)
        {
            ReadOnlyCollection <IWebElement> elements;

            WebDriverWait wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 15));
            elements = wait.Until((driver => _driver.FindElements(by)));
            //Console.WriteLine("2. PageBase.FindElements: " + by.ToString());
            return elements;

        }
    }
}
