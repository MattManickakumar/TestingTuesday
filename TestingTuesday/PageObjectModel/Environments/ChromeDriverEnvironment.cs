using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PageObjectModel
{
    public class ChromeDriverEnvironment:IDriverEnvironment
    {
        private int SyncTime = 120000;
        public ChromeDriverEnvironment()
        {
            //default time - 2 minutes;
        }
        public ChromeDriverEnvironment(int _UserSetTime)
        {
            SyncTime = _UserSetTime;
        }

        public IWebDriver CreateWebDriver()
        {
            try
            {
                var options = new ChromeOptions();
                
                options.AddExcludedArguments("ignore-certificate-errors");                

                options.AddArgument("start-maximized");
                options.AddArguments("--disable-infobars");
                //---------- Headless -------------------
                //options.AddArguments("--headless");                
                //---------------------------------------
                options.AddUserProfilePreference("credentials_enable_service", false);
                options.AddUserProfilePreference("profile.password_manager_enabled", false);

                var chromeDriver = new ChromeDriver(DriverPath, options,TimeSpan.FromMilliseconds(SyncTime));
                chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(2);
                chromeDriver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(2);
                chromeDriver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromMinutes(2);

                Console.WriteLine("Sync Time set for this run : " + SyncTime);
                return chromeDriver;
            }
            catch (Exception e)
            {
                Console.WriteLine("CreateWebDriver Exception message : " + e.Message);
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
                catch (Exception ex) {
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
