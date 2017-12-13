using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using System.Threading;
using PageObjectModel;

namespace PagObjectModel.Setup
{
    public class Session
    {
        public IWebDriver Driver { get; private set; }
        protected int _implicitWait = 2;

        public Session(IDriverEnvironment environment)
        {
            Driver = environment.CreateWebDriver();            
        }

        public string Title
        {
            get
            {
                return Driver.Title;
            }
        }

        public void NavigateTo(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public void Goback()
        {
            Driver.Navigate().Back();
        }

        public void GoForward()
        {
            Driver.Navigate().Forward();
        }

        public void Refresh()
        {
            Driver.Navigate().Refresh();
        }

        public void End()
        {
            Driver.Quit();
        }

        public void CloseBrowser()
        {
            //To do: Sync need to be identified for QA and Lab.

            Thread.Sleep(10000); // Waits for 10 seconds to quit
            Driver.Close();
            Driver.Dispose();
        }
    }
}
