using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace PageObjectModel
{
    public class FirefoxDriverEnvironment : IDriverEnvironment
    {
        private int SyncTime = 120000;
        public FirefoxDriverEnvironment()
        {
            //default time - 2 minutes;
        }
        public FirefoxDriverEnvironment(int _UserSetTime)
        {
            SyncTime = _UserSetTime;
        }

        public IWebDriver CreateWebDriver()
        {
            try
            {                                
                var firefox = new FirefoxDriver();
                firefox.Manage().Window.Maximize();

                firefox.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(SyncTime);
                firefox.Manage().Timeouts().PageLoad = TimeSpan.FromMilliseconds(SyncTime);
                firefox.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromMilliseconds(SyncTime);



                return firefox;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("CreateWebDriver Exception message : " + e.Message);
                throw;
            }
        }

        private string path = string.Empty;

        public string DriverPath
        {
            get
            {
                try
                {
                    path = AppDomain.CurrentDomain.BaseDirectory;
                    return path;
                }
                catch (Exception ex)
                {
                    throw new System.Exception(string.Format("{0} {1}", "Invalid driver path", path), ex);
                }
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
