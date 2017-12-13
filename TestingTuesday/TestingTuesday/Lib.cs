using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagObjectModel.Setup;
using PageObjectModel;
using System.Configuration;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using System.IO;

namespace TestingTuesday
{
    public class Lib
    {
        public Session session;
        public Bilbasen bilbasen;
        public SearchResult searchResult;

        public IWebDriver driver
        {
            get
            {
                return session.Driver;
            }
        }
             

        public void StartSession()
        {
           
            try
            {
                //Browser Type
                //switch (BrowserType.Trim().ToUpper())

                switch (ConfigurationManager.AppSettings["Browser"].Trim().ToUpper())
                {

                    case "CHROME":
                        session = new Session(new ChromeDriverEnvironment(Convert.ToInt32(ConfigurationManager.AppSettings["App_Sync"])));
                        break;
                    case "FIREFOX":
                        session = new Session(new FirefoxDriverEnvironment(Convert.ToInt32(ConfigurationManager.AppSettings["App_Sync"])));
                        break;

                    default:
                        session = new Session(new ChromeDriverEnvironment(Convert.ToInt32(ConfigurationManager.AppSettings["App_Sync"])));
                        break;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error during session start. Err message: " + e.Message);
                throw;
            }

        }

        public void closeSession()
        {

            TakeScreenShot(); //- Commented due to chrome upgrade
            session.CloseBrowser();
            session.End();            
        }
        public void TakeScreenShot()
        {
            var takesScreenshot = ((ITakesScreenshot)driver);
            if (takesScreenshot != null)
            {

                var screenshot = takesScreenshot.GetScreenshot();
                var tempFileName = Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ".jpg";
                var tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), tempFileName);                
                screenshot.SaveAsFile(tempFilePath, ScreenshotImageFormat.Jpeg);

                Console.WriteLine($"SCREENSHOT[ file:///{tempFileName} ]SCREENSHOT");

            }
        }
    }

    [Binding]
    public class Reporting
    {

        private readonly Lib lib;

        public Reporting(Lib _lib)
        {
            if (FeatureContext.Current.ContainsKey("lib"))
            {
                lib = FeatureContext.Current.Get<Lib>("lib");
            }
            else lib = _lib;

        }


        [AfterStep()] // - Commented due to chrome upgrade
        public void MakeScreenshotAfterStep()
        {
            
                var driver = lib.driver;

                var takesScreenshot = ((ITakesScreenshot)driver);
                if (takesScreenshot != null)
                {

                    var screenshot = takesScreenshot.GetScreenshot();
                    var tempFileName = Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ".jpg";
                    var tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), tempFileName);                    
                    screenshot.SaveAsFile(tempFilePath, ScreenshotImageFormat.Jpeg);
                    Console.WriteLine($"SCREENSHOT[ file:///{tempFileName} ]SCREENSHOT");

                }         

        }

    }
}
