using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Praksys.PageObjects;
using Praksys.PageObjects.Core.Setup;
using Praksys.PageObjects.PageLayout;
using Praksys.PageObjects.Security;
using System;
using System.IO;
using System.Drawing.Imaging;
using System.Configuration;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Praksys.PageObjects.Core;
using Praksys.PageObjects.UnionAndAgreements;
using Praksys.PageObjects.Provider;
using Praksys.PageObjects.Payment;
using OpenQA.Selenium.Support.UI;
using Praksys.PageObjects.Core.Controls;
using System.Threading;
using System.Collections.Specialized;
using OpenQA.Selenium.Firefox;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using Praksys.PageObjects.Claims;
using Microsoft.WindowsAPICodePack.Shell;
using excel = Microsoft.Office.Interop.Excel;
using Praksys.PageObjects.ProviderSelfService;
using TechTalk.SpecFlow.Assist;
using System.Diagnostics;
using OpenQA.Selenium.Interactions;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;
using WhiteUIItem = TestStack.White.UIItems;
using System.Windows.Automation;
using System.Globalization;
using OpenQA.Selenium.Chrome;
using Praksys.PageObjects.Integration;

namespace Praksys.Business.ComponentTest.StepDefinitions
{

    public class PraksysLib : PraksysPages
    {
        private IWebDriver _driver;
        private Tabs tabs;
        private Dictionary<string, string> TA_Memory = new Dictionary<string, string>();
        public bool Flag_LogOff;
        private NameValueCollection _config = ConfigurationManager.AppSettings;
        private NemID nemID;
        //private int _HFimplicitwait = Configuration.Ma;
        private string _userFirstName, _userLastName, _userEmail;
        Random rnd = new Random();

        private List<Process> FF; //List of firefox instance before the start of run.
        private List<Process> CH; //List of chrome instance before the start of run.

        public PraksysLib()
        {
            FF = Process.GetProcessesByName("firefox").ToList();
            CH = Process.GetProcessesByName("chrome").ToList();
        }

        public IWebDriver getDriver
        {
            get { return _driver; }
        }

        public List<Process> GetFirefoxProcessBR
        {
            get
            {
                return FF;
            }
        }

        public List<Process> GetChromeProcessBR
        {
            get
            {
                return CH;
            }
        }
        public string Browser
        {
            get
            {
                return ConfigurationManager.AppSettings["Browser"];
            }
        }

        public void SwitchToPraksysFrame()
        {

            _driver.SwitchTo().ParentFrame();
        }


        public string UserFirstName
        {
            get { return _userFirstName; }
        }
        public string UserLastName
        {
            get { return _userLastName; }
        }
        public string UserEmail
        {
            get { return _userEmail; }
        }

        public string FilePath
        {
            get { return AppDomain.CurrentDomain.BaseDirectory + @"\Files\"; }
        }

        public string DownloadPath
        {
            get { return KnownFolders.Downloads.Path + "\\"; }
        }
        public string Root_FilePath
        { //file path within the project
            get { return Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\" + @"\Files\"); }
        }

        public string SikuliPath
        {
            get { return AppDomain.CurrentDomain.BaseDirectory + @"\Files\Sikuli\"; }
        }
        public PageObjects.Core.Setup.Session getsession
        {
            get { return session; }
        }

        public CultureInfo getCultureInfo
        {
            get { return new CultureInfo("da-DK", true); }
        }

        public string GetRegion
        {
            //returns user logged in regionCode as string
            get
            {
                var header = new Header(_driver);
                return header.GetRegion;
            }
        }

        public string GetRegionName
        {
            //returns user logged in regionCode as string
            get
            {
                var header = new Header(_driver);
                return header.GetRegionName;
            }
        }

        public string GetKommune
        {
            //returns a kommune which is part of the user logged in region
            get
            {
                var header = new Header(_driver);
                return header.GetKommune;
            }
        }

        public string GetEDInumber
        {
            get
            {
                switch (GetRegionName.ToUpper())
                {
                    case "REGION NORDJYLLAND":
                        return EDInumbers.RegionNordjylland;
                    case "REGION MIDTJYLLAND":
                        return EDInumbers.RegionMidtjylland;
                    case "REGION SYDDANMARK":
                        return EDInumbers.RegionSyddanmark;
                    case "REGION HOVEDSTADEN":
                        return EDInumbers.RegionHovestaden;
                    case "REGION SJÆLLAND":
                        return EDInumbers.RegionSjaelland;
                    default:
                        return "Error identifying the region";
                }
            }
        }

        public Tabs Tab
        {
            get { return tabs; }
        }
        public void Login()
        {
            try
            {
                StartSession();
                var _config = ConfigurationManager.AppSettings;

                Assert.IsTrue(securityPage.Login(_config["UserName"], _config["Password"], _config["Environment"]));

                _driver = session.Driver;
                Flag_LogOff = false;

                var UserDetails = _config["UserName"].Split(';'); //User Details
                _userEmail = UserDetails[0];
                _userFirstName = UserDetails[1];
                _userLastName = UserDetails[2];
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error during login. Err message: " + e.Message);
                throw;
            }
        }

        public void Login(string profile)
        {
            try
            {

                StartSession();


                string userName = _config["UserName"];  //default profile
                string password = _config["Password"];

                if (profile.ToUpper().Contains("PROFILE"))
                {
                    var _profile = profile.Split('_');
                    userName = _config[_profile[0] + "_UserName"];

                    password = _config[_profile[0] + "_Password"];
                }

                Assert.IsTrue(securityPage.Login(userName, password, _config["Environment"]));
                Flag_LogOff = false;
                _driver = session.Driver;

                var UserDetails = userName.Split(';'); //User Details

                _userEmail = UserDetails[0];
                _userFirstName = UserDetails[1];
                _userLastName = UserDetails[2];

            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error during login. Err message: " + e.Message);
                throw;
            }
        }

        public void LogIn_YderSelvbetjening()
        {
            try
            {

                StartSession("Firefox");

                var _config = ConfigurationManager.AppSettings;

                session.NavigateTo(_config["SelfServiceURL"]);
                //session.NavigateTo("https://yderselvbetjeningoffshore.praksyslab.dk/Home/Login");

                System.Console.WriteLine("Selvbetjening:" + _config["SelfServiceURL"]);

                nemID = new NemID(session.Driver);

                Assert.IsTrue(nemID.Login(_config["NemID_UserName"], _config["NemID_Password"], _config["DanIDURL"]));

                _driver = session.Driver;
                _driver.SwitchTo().ParentFrame();

                Flag_LogOff = false;
            }

            catch (Exception e)
            {
                Console.WriteLine("Error during login. Err message: " + e.Message);
                throw;
            }
        }

        public void LogIn_YOA()
        {
            try
            {
                StartSession("Firefox");

                var _config = ConfigurationManager.AppSettings;

                session.NavigateTo(_config["AOYURL"]);

                Console.WriteLine("Selvbetjening:" + _config["AOYURL"]);

                nemID = new NemID(session.Driver);

                Assert.IsTrue(nemID.Login(_config["NemID_UserName"], _config["NemID_Password"], _config["DanIDURL"]));

                _driver = session.Driver;
                _driver.SwitchTo().ParentFrame();

                Flag_LogOff = false;
            }

            catch (Exception e)
            {
                System.Console.WriteLine("Error during login. Err message: " + e.Message);
                throw;
            }
        }

        public void Hangfire()
        {
            try
            {
                NavigateTo("Hangfire");

                _driver.SwitchTo().Frame("HangsFire"); //Switching to Hangfire frame

                _driver.FindElement(By.PartialLinkText("Recurring Jobs")).Click();
                var y = _driver.FindElement(By.XPath("//*[contains(@data-metric,'recurring:count')]")).Text;
                _driver.FindElement(By.PartialLinkText("500")).Click();
                string[] Team = new string[Convert.ToInt16(y)];

                for (int i = 1; i < Convert.ToInt16(y); i++)
                {
                    var xpath = "//*[@id='wrap']//table/tbody/tr[" + i + "]/td[2]";
                    var x = _driver.FindElement(By.XPath(xpath)).Text;
                    Team[i - 1] = x;
                    if ("checkInactiveProviderJob" == x) // change the text according to your search
                    {
                        var Xpath = "//*[@id='wrap']//table/tbody/tr[" + i + "]/td[1]/input";
                        _driver.FindElement(By.XPath(Xpath)).Click();
                    }
                }
            }

            catch (Exception e)
            {
                System.Console.WriteLine("Error during Hangfire. Err message: " + e.Message);
                closeSession();
                throw;
            }

        }

        public void TriggerHFJob(string JobName)
        {
            try
            {
                NavigateTo("Hangfire");

                var hangfire = new Hangfire(_driver, Tabs(hangfire_Title, true));
                hangfire.Recurring_Jobs.Click();
                hangfire.MaxItemPerPage.Click();

                var table = hangfire.RJ_Table; //Recurring Jobs table

                var columnHeader = table.FindElements(By.XPath("./thead//th"));
                int Index_ID = columnHeader.ToList().FindIndex(id => id.Text.Equals("id", StringComparison.OrdinalIgnoreCase));
                Index_ID++;
                var Jobs = hangfire.RJ_Table.FindElements(By.XPath("./tbody/tr/td[" + Index_ID.ToString() + "]"));
                int Index_Job = Jobs.ToList().FindIndex(job => job.Text.Equals(JobName, StringComparison.OrdinalIgnoreCase));

                if (Index_Job != -1)
                {

                    table.FindElement(By.XPath("./tbody/tr[" + (Index_Job + 1) + "]/td[1]/input")).Click(); // Select the checkbox
                    IJavaScriptExecutor jse = (IJavaScriptExecutor)_driver;     // scroll cursur up
                    jse.ExecuteScript("scroll(0,0)", "");
                    hangfire.Trigger.Click();
                    Console.WriteLine("Job - " + JobName + " has been triggered.");
                    Thread.Sleep(5000); //Sleep for 5 seconds.
                }
                else Console.WriteLine("Job - '" + JobName + "' not found. Hence not triggered.");

                _driver.SwitchTo().DefaultContent();

                Tab.CloseTabByTitle(hangfire_Title, true);



            }
            catch (Exception e)
            {
                Console.WriteLine("Error during Hangfire. Err message: " + e.Message);
                closeSession();
                throw;
            }
        }

        public void NavigateTo(string PageName)
        {

            try
            {
                menu = new Menu(_driver);
                switch (PageName.ToUpper())
                {
                    //1. Aftaler og Regler   
                    case "LOKALAFTALER":
                        if (!menu.AftalerParent.AreaExpanded) menu.AftalerParent.Click();
                        menu.LokalAftaler.Click();
                        break;

                    case "AFTALER":
                        if (!menu.AftalerParent.AreaExpanded) menu.AftalerParent.Click();
                        menu.Aftaler.Click();
                        break;

                    case "YDELSESGRUPPERINGER":
                        if (!menu.AftalerParent.AreaExpanded) menu.AftalerParent.Click();
                        menu.YdelseGrupperinger.Click();
                        break;

                    case "REGELSÆT":
                        if (!menu.AftalerParent.AreaExpanded) menu.AftalerParent.Click();
                        menu.Regelsaet.Click();
                        break;

                    case "REGELFAMILIER":
                        if (!menu.AftalerParent.AreaExpanded) menu.AftalerParent.Click();
                        menu.RegelFamilier.Click();
                        break;


                    //2. Økonomi
                    case "KONTOPLANER":
                        if (!menu.KontoplanerParent.AreaExpanded) menu.KontoplanerParent.Click();
                        menu.Kontoplaner.Click();
                        break;

                    // UDBATELING
                    case "GODKEND":
                        //if (!menu.GodkendParent.AreaExpanded) menu.GodkendParent.Click();
                        menu.Godkend.Click();
                        break;

                    //case "UDBETALINGER":
                    //    //if (!menu.GodkendParent.AreaExpanded) menu.GodkendParent.Click();
                    //    menu.Udbetalinger.Click();
                    //    break;

                    //case "EARKIV":
                    //    menu.eArkiv.Click();
                    //    break;


                    //3. Self Service
                    case "AFMELD YDERNUMMER (AFGANGSFORING)":
                        menuselfservice.Afmeld_ydernummer.Click();
                        menuselfservice.SelfServiceSync();
                        menuselfservice.SelfServiceSync();
                        break;

                    //4. Yder
                    case "YDER":
                        if (!menu.YderParent.AreaExpanded) menu.YderParent.Click();
                        menu.Yder.Click();
                        break;

                    case "HÆNDELSESOVERSIGT":
                        if (!menu.YderParent.AreaExpanded) menu.YderParent.Click();
                        menu.Haendelsesoversigt.Click();
                        break;
                    case "FAELLESSKAB":
                        if (!menu.YderParent.AreaExpanded) menu.YderParent.Click();
                        menu.Faellesskab.Click();
                        break;

                    case "YDERINFOTYPE":
                        if (!menu.YderParent.AreaExpanded) menu.YderParent.Click();
                        menu.Yderinfotype.Click();
                        break;

                    //4. Klassifikation
                    case "KLASSIFIKATIONER":
                        if (!menu.KlassifikationParent.AreaExpanded) menu.KlassifikationParent.Click();
                        menu.Klassifikationer.Click();
                        break;

                    //5. Earchive
                    case "EARKIV":
                        menu.eArkiv.Click();
                        break;

                    //6. Konfiguration                
                    case "MYNDIGHED":
                        if (!menu.Konfiguration.AreaExpanded) menu.Konfiguration.Click();
                        menu.Myndighed.Click();
                        break;

                    case "HÆNDELSER":
                        if (!menu.Konfiguration.AreaExpanded) menu.Konfiguration.Click();
                        menu.Haendelser.Click();
                        break;

                    case "DOKUMENTTYPER":
                        if (!menu.Konfiguration.AreaExpanded) menu.Konfiguration.Click();
                        menu.Dokumenttyper.Click();
                        break;

                    case "FORRETNINGSREGLER":
                        if (!menu.Konfiguration.AreaExpanded) menu.Konfiguration.Click();
                        menu.Forretningsregler.Click();
                        break;

                    case "SAGER":
                        if (!menu.Konfiguration.AreaExpanded) menu.Konfiguration.Click();
                        menu.Sager.Click();
                        break;

                    case "BESKEDER":
                        if (!menu.Konfiguration.AreaExpanded) menu.Konfiguration.Click();
                        menu.Beskeder.Click();
                        break;

                    case "SKUL YDELSE":
                        if (!menu.Konfiguration.AreaExpanded) menu.Konfiguration.Click();
                        menu.Skjul_Ydelse.Click();
                        break;


                    //  ---    Sys Adminstation    ---

                    case "SYSADMIN_FORRETNINGSREGLER":
                        menu.SysAdmin_Forretningsregler.Click();
                        break;

                    //5. Hangfire
                    case "HANGFIRE":
                        menu.Hangfire.Click();
                        break;

                    case "RABBIT_MQ":
                        menu.RabbitMQ.Click();
                        break;

                    case "OIP":
                        menu.OIP.Click();
                        break;

                    case "ECRION":
                        menu.Ecrion.Click();
                        break;

                    //---   REGNINGSBEHANDLING

                    //9. Claims
                    case "REGNINGSOVERBLIK":
                        menu.Regningsoverblik.Click();
                        break;

                    //6. Bundter
                    case "BUNDTER":
                        menu.Bundter.Click();
                        break;

                    case "REGNINGER":
                        menu.Regninger.Click();
                        break;

                    case "HENVISNINGER":
                        menu.Henvisninger.Click();
                        break;

                    //8. Reguleringer
                    case "REGULERINGER":
                        menu.Reguleringer.Click();
                        break;
                    //------------------------------------------------------------------
                    //Afregning
                    //7. Udbetalingsgrundlag
                    case "UDBETALINGSGRUNDLAG":
                        if (!menu.Afregning_Section.IsExpanded) menu.Afregning_Section.Click();
                        menu.Udbetalingsgrundlag.Click();
                        break;
//-------------------------------------------------------------------
                    //Udbetaling
                    case "GODKEND_UDBETALING":
                        if (!menu.Udbetaling_Section.IsExpanded) menu.Udbetaling_Section.Click();
                        menu.GodkendUdbetaling.Click();
                        break;

                    case "UDBETALINGER":
                        if (!menu.Udbetaling_Section.IsExpanded) menu.Udbetaling_Section.Click();
                        menu.Udbetalinger.Click();
                        break;

                    //-------------------------------------------------------------------



                    default:
                        Console.WriteLine("Error in the NavigateTo() method call. Parameter " + PageName + "doesnt exist.");
                        break;
                }


                //Synchronise
                menu.Sync();

            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error during NavigateTo(). Err message: " + e.Message);
                closeSession();
                throw;
            }


        }

        public void NavigateToYS(string PageName)
        {

            try
            {
                menuselfservice = new PageObjects.ProviderSelfService.Menuselfservice(_driver);
                switch (PageName.ToUpper())
                {
                    // Self Service
                    case "AFMELD YDERNUMMER (AFGANGSFORING)":
                        menuselfservice.Afmeld_ydernummer.Click();
                        break;

                    case "VIS RET YDEROPLYSNINGER":
                        menuselfservice.Visretyderoplysninger.Click();
                        break;

                    case "SIKREDE SAMT ABEN LUKKET STATUS":
                        menuselfservice.Sikrede_samt.Click();
                        break;

                    case "VIKARUDBETALING MENU":
                        menuselfservice.Vikarudbetaling.Click();
                        break;

                    case "OPRET REGNING":
                        menuselfservice.Opretregning.Click();
                        break;

                    case "OPRET NY YDERPERSON":
                        menuselfservice.Opret_ny_Yderperson.Click();
                        break;

                    case "OPRET NY SATELLIT":
                        menuselfservice.Opret_ny_satellite.Click();
                        break;

                    case "AMELD YDERNUMMER":
                        menuselfservice.Ameld_ydernummer.Click();
                        break;

                    case "KONTAKT REGIONEN":
                        menuselfservice.Kontakt_regionen.Click();
                        break;

                    case "TIMELD AFMELD MAIL ADVISERING":
                        menuselfservice.Timeld_afmeld_mail_advisering.Click();
                        break;

                    case "VEJLEDNING":
                        menuselfservice.Vejledning.Click();
                        break;

                    case "HISTORIK MENU":
                        menuselfservice.Historik.Click();
                        break;



                    default:
                        Console.WriteLine("Error in the NavigateTo() method call. Parameter " + PageName + "doesnt exist.");
                        break;
                }


                //Synchronise
                menuselfservice.SelfServiceSync();
                menuselfservice.SelfServiceSync();

            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error during NavigateTo(). Err message: " + e.Message);
                closeSession();
                throw;
            }


        }

        public void Logout()
        {
            try
            {
                header = new Header(_driver);
                header.Logout.Click();
                session.CloseBrowser();
                session.End(); // To be tested. - Performance Test
                Flag_LogOff = true;
            }
            catch (Exception)
            {
                closeSession();
                throw;
            }
        }

        public void Logout_AOY()
        {
            try
            {
                AOY = new AOY_ProviderSignUp(_driver);
                AOY.Logout.Click();
                session.CloseBrowser();
                Flag_LogOff = true;
            }
            catch (Exception)
            {
                closeSession();
                throw;
            }
        }

        public void Logout_YS()
        {
            try
            {
                headerselfservice = new PageObjects.ProviderSelfService.Headerselfservice(_driver);
                headerselfservice.Logout.Click();
                session.CloseBrowser();
                Flag_LogOff = true;
            }
            catch (Exception)
            {
                closeSession();
                throw;
            }
        }

        public void closeSession()
        {

            TakeScreenShot(); //- Commented due to chrome upgrade
            session.CloseBrowser();
            session.End();
            Flag_LogOff = true;
        }

        public void TakeScreenShot()
        {
            var takesScreenshot = ((ITakesScreenshot)_driver);
            if (takesScreenshot != null)
            {

                var screenshot = takesScreenshot.GetScreenshot();
                var tempFileName = Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ".jpg";
                var tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), tempFileName);
                //screenshot.SaveAsFile(tempFilePath, ImageFormat.Jpeg);
                screenshot.SaveAsFile(tempFilePath, ScreenshotImageFormat.Jpeg);

                Console.WriteLine($"SCREENSHOT[ file:///{tempFileName} ]SCREENSHOT");

            }
        }

        private void StartSession()
        {

            try
            {
                //Browser Type
                switch (ConfigurationManager.AppSettings["Browser"].ToUpper())
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

                session.NavigateTo(ConfigurationManager.AppSettings["URL"]);
                Console.WriteLine("Environment:" + ConfigurationManager.AppSettings["Environment"]);
                Console.WriteLine("url:" + ConfigurationManager.AppSettings["URL"]);
                securityPage = new SecurityPage(session.Driver);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error during session start. Err message: " + e.Message);
                throw;
            }

        }

        private void StartSession(String BrowserType)
        {

            try
            {
                //Browser Type
                switch (BrowserType.Trim().ToUpper())
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
                System.Console.WriteLine("Error during session start. Err message: " + e.Message);
                throw;
            }

        }

        public IWebElement Tabs(string Title, bool PartialSearch)
        {
            IWebElement pagecontent;
            tabs = new Tabs(_driver);
            /*Assert.IsTrue(tabs.GetTabIndex(Title, true) >= 0, Title + " tab is not opened");
            System.Console.WriteLine(Title + " tab is opened");
            pagecontent = tabs.GetTabContent(Title, true);            
            */

            pagecontent = tabs.GetTabContent(Title, true);
            Assert.IsTrue(pagecontent != null, Title + " tab is not opened");
            Console.WriteLine(Title + " tab is opened");
            return pagecontent;

        }

        public IWebElement TabText(string Text, bool PartialSearch)
        {
            IWebElement pagecontent;
            tabs = new Tabs(_driver);
            pagecontent = tabs.GetTabTextContent(Text, true);
            Assert.IsTrue(pagecontent != null, Text + " tab is not opened");
            System.Console.WriteLine(Text + " tab is opened");
            return pagecontent;

        }

        public void Remember(string key, string value)
        {
            if (!TA_Memory.ContainsKey(key)) TA_Memory.Add(key, value);
        }

        public string GetMe(string key)
        {
            return TA_Memory[key]; // recollect those stored memory.
        }

        public string GetCPR()
        {
            //returns - the cpr as string from cpr.xlsx file. 
            //return  - null if cpr list is empty.
            string cpr = null;
            try
            {
                //Getting Cpr No. from excel sheet
                excel.Application app = new excel.Application();
                excel.Workbook workbook;

                if (System.IO.File.Exists(Root_FilePath + "Cpr.xlsx")) workbook = app.Workbooks.Open(Root_FilePath + "Cpr.xlsx");   //during dev- CPR.xlsx present in root folder is updated.
                else workbook = app.Workbooks.Open(FilePath + "Cpr.xlsx"); //During execution - CPR.xlsx present in bin folder is updated.

                excel.Worksheet sheet = workbook.Sheets[1];
                excel.Range range = sheet.UsedRange;

                int countrecord = range.Rows.Count;

                for (int i = 2; i < countrecord; i++)
                {
                    for (int j = 2; j < countrecord; j++)
                    {
                        if (sheet.Cells[2][j].value2 == "x")
                        {
                            i++;
                        }
                    }
                    sheet.Cells[2][i].value2 = "x";
                    cpr = Convert.ToString(range.Cells[1][i].value2);
                    System.Console.WriteLine("CPR NO. :" + cpr);
                    break;
                }
                workbook.Save();
                workbook.Close(true, Type.Missing, Type.Missing);
                app.Quit();
                return cpr;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error retrieving cpr number from CPR.xlsx file. Error message " + e.Message);
                return "#Error";
            }
        }

        public string Get_CPR_SelfService()
        {
            //returns - the cpr as string from cpr.xlsx file. 
            //return  - null if cpr list is empty.
            string cpr = null;
            try
            {
                //Getting Cpr No. from excel sheet
                excel.Application app = new excel.Application();
                excel.Workbook workbook;

                if (System.IO.File.Exists(Root_FilePath + "Cpr.xlsx")) workbook = app.Workbooks.Open(Root_FilePath + "Cpr.xlsx");   //during dev- CPR.xlsx present in root folder is updated.
                else workbook = app.Workbooks.Open(FilePath + "Cpr.xlsx"); //During execution - CPR.xlsx present in bin folder is updated.

                excel.Worksheet sheet = workbook.Sheets[1];
                excel.Range range = sheet.UsedRange;

                int countrecord = range.Rows.Count;

                for (int i = 2; i < countrecord; i++)
                {
                    for (int j = 2; j < countrecord; j++)
                    {
                        if (sheet.Cells[3][j].value2 == "x")
                        {
                            i++;
                        }
                    }
                    sheet.Cells[3][i].value2 = "x";
                    cpr = Convert.ToString(range.Cells[1][i].value2);
                    System.Console.WriteLine("CPR NO. :" + cpr);
                    break;
                }
                workbook.Save();
                workbook.Close(true, Type.Missing, Type.Missing);
                app.Quit();
                return cpr;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error retrieving cpr number from CPR.xlsx file. Error message " + e.Message);
                return "#Error";
            }
        }

        public string RandomCPR()
        {
            return DateTime.Now.AddYears(-20).ToString("ddMMyy") + rnd.Next(1000, 9999).ToString();
        }

        public void HangfireSync()
        {
            try
            {
                /*
                 * Synchronize the automation run based on the background hangfire call.
                 * Note : This method is designed to be called from all classes within 'PageObjects' namespace.
                 */
                System.Console.WriteLine("Initiated the background job, awaiting for the completion notification...");

                string _HFimplicitwait = _config["Hangfire_MaxWaitingTime"];


                WebDriverWait wait = new WebDriverWait(_driver, new TimeSpan(0, int.Parse(_HFimplicitwait), 0));
                IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@data-notify='container'][@role='alert']")));


                //Read the message
                var msg = new PageObjects.Core.Controls.Span(element.FindElement(By.XPath(@"./span")));
                System.Console.WriteLine("Hangfire Notification message: " + msg.Text);
                Thread.Sleep(1000);
                TakeScreenShot();

                //Close the notification.
                var close = new Button(element.FindElement(By.XPath(@"./button")));
                close.Click();


            }
            catch (Exception)
            {
                System.Console.WriteLine("Delay in the reporting the Hangfire notification. Test automation has waited for " + int.Parse(_config["Hangfire_MaxWaitingTime"]) + " minutes");
                closeSession();
            }

        }

        public string CreateProvider()
        {
            /*
             * Objective : To support in creation of precondition test data. 
             * Note : Currently the script creates a Physiotherapist with basic details.
             * Testdata : Hard coded
             */

            try
            {
                // 01. Navigate to Yder
                NavigateTo("Yder");


                //02.  Opret Ny - Click
                var searchYder = new SearchYder(_driver, Tabs(searchYder_Title, true));
                searchYder.OpretNy.Click();
                searchYder.Sync();

                //03. Enter the category as Fysioterapeut
                providerCategoryPopup = new ProviderCategoryPopup(_driver);
                providerCategoryPopup.GyldigFra.Clear();
                providerCategoryPopup.GyldigFra.Type(DateTime.Now.ToString("dd-MM-yyyy"));
                providerCategoryPopup.YderKategori.SelectByValue(_driver, "Fysioterapeut");
                providerCategoryPopup.Sync();
                providerCategoryPopup.Ok.Click();
                searchYder.Sync();
                System.Console.WriteLine("Fysioterapeut Catergory selected.");

                //04. Validate the Opret ny yder page is opened
                provider = new PageObjects.Provider.Provider(_driver, Tabs(provider_Title, true));


                //05. Enter Basic Details
                provider.YderBasicDetail.Yderbetegnelse.Type("Specflow Yder");
                if (provider.YderBasicDetail.Nemkonto.Enabled)
                {
                    provider.YderBasicDetail.Nemkonto.Type("44444444");
                }
                provider.YderBasicDetail.AdgangsType.SelectByValue(_driver, "CPR-nr.");
                //provider.YderBasicDetail.Adgangsid.Type("44444444");
                string cprNumber = _config["SelfService_cprNumber"];
                provider.YderBasicDetail.Adgangsid.Type(cprNumber);
                provider.YderBasicDetail.JurdiskType.SelectByValue(_driver, "CVR-nr.");
                provider.YderBasicDetail.Jurdiskid.Type("44444444");
                provider.YderBasicDetail.Praksisform.SelectByValue(_driver, "Kompagniskabspraksis");
                provider.YderBasicDetail.TildeltKapacitet.Type("00");
                provider.YderBasicDetail.TildeltKapacitetDropDown.SelectByValue(_driver, "00");
                provider.YderBasicDetail.Praksistype.SelectByValue(_driver, "Lejer af kapacitet (klinik)");
                provider.YderBasicDetail.KontaktEmail.Type("abcd@csc.com");
                provider.YderBasicDetail.Hjemmeside.Type("http://www.csc.com");
                provider.YderBasicDetail.Hovedspeciale.SelectByValue(_driver, "80-Almen praksis");
                provider.YderBasicDetail.HovedTelefone.Type("9999999999");
                provider.YderBasicDetail.HovedOffentlig.Click();
                provider.YderBasicDetail.Tilgangsdato.Clear();
                provider.YderBasicDetail.Tilgangsdato.Type(DateTime.Now.ToString("dd-MM-yyyy"));
                provider.Sync();

                //06. close the YderAccordion
                provider.YderAccordion.Click();
                provider.Sync();

                Console.WriteLine("Basic details of provider entered.");

                //07. Open the Lokation accordion
                if (!provider.LokationAccordion.IsExpanded)
                    provider.LokationAccordion.Click();
                provider.Sync();

                //08. Enters the Lokation details
                provider.LokationAddButton.Click();
                provider.CurrentLokation.Postnr.SelectByValue(_driver, "1052 KØBENHAVN K");

                provider.CurrentLokation.Vejnavn.SelectByValue(_driver, "Herluf Trolles Gade");
                provider.CurrentLokation.Husnr.Type("321");
                provider.CurrentLokation.Etage.Type("22");
                provider.CurrentLokation.SideDoor.Type("tvf");
                provider.CurrentLokation.Adressesupplement.Type("Test");
                provider.CurrentLokation.Lokation_Ok.Click();
                provider.Sync();
                provider.LokationAccordion.Click(); // Close Location accordion
                Console.WriteLine("Location details of provider entered.");

                //09. Create the provider
                provider.Gem.Click();
                provider.Sync();

                while (provider.ToasterTitle != "Information")
                {
                    provider.ToasterOk.Click();
                    Console.WriteLine("Confirmed to proceed with creation of Provider");
                }

                var msg = provider.ToasterMsg.Text;

                string Ytext = "ydernummer";
                int starposition = msg.IndexOf(Ytext) + Ytext.Length;
                var providerNr = msg.Substring(starposition, msg.Length - starposition);

                provider.Sync();

                provider.ToasterOk.Click();
                provider.Sync();

                //10. close the Search provider tab
                Tab.CloseTabByTitle(searchYder_Title, true);

                return providerNr;
            }
            catch (Exception)
            {
                return "Error during provider creation";
                session.CloseBrowser();
                throw;
            }


        }
        public string CreateAlmenProvider()
        {
            /*
             * Objective : To support in creation of precondition test data. 
             * Note : Currently the script creates a Physiotherapist with basic details.
             * Testdata : Hard coded
             */

            try
            {
                // 01. Navigate to Yder
                NavigateTo("Yder");


                //02.  Opret Ny - Click
                var searchYder = new SearchYder(_driver, Tabs(searchYder_Title, true));
                searchYder.OpretNy.Click();
                searchYder.Sync();

                //03. Enter the category as Fysioterapeut
                providerCategoryPopup = new ProviderCategoryPopup(_driver);
                providerCategoryPopup.GyldigFra.Clear();
                providerCategoryPopup.GyldigFra.Type(DateTime.Now.ToString("dd-MM-yyyy"));
                providerCategoryPopup.YderKategori.SelectByValue(_driver, "Almenlæge");
                providerCategoryPopup.Sync();
                providerCategoryPopup.Ok.Click();
                searchYder.Sync();
                System.Console.WriteLine("Almen Catergory selected.");

                //04. Validate the Opret ny yder page is opened
                provider = new PageObjects.Provider.Provider(_driver, Tabs(provider_Title, true));


                //05. Enter Basic Details
                provider.YderBasicDetail.Yderbetegnelse.Type("Specflow Yder");
                if (provider.YderBasicDetail.Nemkonto.Enabled)
                {
                    provider.YderBasicDetail.Nemkonto.Type("44444444");
                }
                provider.YderBasicDetail.AdgangsType.SelectByValue(_driver, "CPR-nr.");
                //provider.YderBasicDetail.Adgangsid.Type("44444444");
                string cprNumber = _config["SelfService_cprNumber"];
                provider.YderBasicDetail.Adgangsid.Type(cprNumber);
                provider.YderBasicDetail.JurdiskType.SelectByValue(_driver, "CVR-nr.");
                provider.YderBasicDetail.Jurdiskid.Type("44444444");
                provider.YderBasicDetail.Praksisform.SelectByValue(_driver, "Kompagniskabspraksis");
                provider.YderBasicDetail.TildeltKapacitet.Type("1");
                provider.YderBasicDetail.KontaktEmail.Type("abcd@csc.com");
                provider.YderBasicDetail.Hjemmeside.Type("http://www.csc.com");
                provider.YderBasicDetail.Hovedspeciale.SelectByValue(_driver, "80-Almen praksis");
                provider.YderBasicDetail.HovedTelefone.Type("123456");
                provider.YderBasicDetail.HovedOffentlig.Click();
                provider.YderBasicDetail.Tilgangsdato.Clear();
                provider.YderBasicDetail.Tilgangsdato.Type(DateTime.Now.ToString("dd-MM-yyyy"));
                provider.Sync();

                //06. close the YderAccordion
                provider.YderAccordion.Click();
                provider.Sync();

                System.Console.WriteLine("Basic details of provider entered.");

                //07. Open the Lokation accordion
                if (!provider.LokationAccordion.IsExpanded)
                    provider.LokationAccordion.Click();
                provider.Sync();

                //08. Enters the Lokation details
                provider.LokationAddButton.Click();
                provider.CurrentLokation.Postnr.SelectByValue(_driver, "1052 K?BENHAVN K");
                provider.Sync();
                provider.Sync();
                provider.CurrentLokation.Vejnavn.SelectByValue(_driver, "Herluf Trolles Gade");
                provider.Sync();
                provider.CurrentLokation.Husnr.Type("321");
                provider.CurrentLokation.Etage.Type("22");
                provider.CurrentLokation.SideDoor.Type("tvf");
                provider.CurrentLokation.Adressesupplement.Type("Test");
                provider.CurrentLokation.Lokation_Ok.Click();
                provider.Sync();
                provider.LokationAccordion.Click(); // Close Location accordion
                System.Console.WriteLine("Location details of provider entered.");

                if (!provider.SikredeAccordion.IsExpanded) provider.SikredeAccordion.Click();

                var sikrede = provider.SikredeDetail;
                if (!sikrede.Individuel.Checked) sikrede.Individuel.Click();
                sikrede.PraksisMin.Clear();
                sikrede.PraksisMin.Type("0");

                sikrede.PraksisMax.Clear();
                sikrede.PraksisMax.Type("10");

                if (!provider.ovrigeyderAccordion.IsExpanded) provider.ovrigeyderAccordion.Click();

                var ovrigeyder = provider.OvrigeYder;
                if (!ovrigeyder.Regningsindberetning.Checked) ovrigeyder.Regningsindberetning.Click();

                //09. Create the provider
                provider.Gem.Click();
                provider.Sync();

                while (provider.ToasterTitle != "Information")
                {
                    provider.ToasterOk.Click();
                    System.Console.WriteLine("Confirmed to proceed with creation of Provider");
                }


                var msg = provider.ToasterMsg.Text;

                string Ytext = "ydernummer";
                int starposition = msg.IndexOf(Ytext) + Ytext.Length;
                var providerNr = msg.Substring(starposition, msg.Length - starposition);
                Remember("providerNr", providerNr);
                provider.Sync();

                provider.ToasterOk.Click();
                provider.Sync();

                //10. close the Search provider tab
                Tab.CloseTabByTitle(searchYder_Title, true);

                return providerNr;
            }
            catch (Exception)
            {
                return "Error during provider creation";
                session.CloseBrowser();
                throw;
            }


        }



        public string CreateSag()
        {
            /*
             * Objective : To support in creation of precondition test data. 
             * Note : Currently the script creates a Physiotherapist with basic details.
             * Testdata : Hard coded
             */

            try
            {
                ////00.Select tab
                //var Tab = new Tabs(_driver);
                //Tab.SelectTabByTitle(hjem_Title,true);

                //01.  Opret Ny - Click
                var hjem = new Hjem(_driver, Tabs(hjem_Title, true));
                hjem.Opretny.Click();
                hjem.Sync();

                //02. Validate the Ad hoc-sag page is opened
                adhoc = new Adhoc(_driver, Tabs(adhoc_Title, true));

                //03. Enter the Deadline and adding Task-1
                //adhoc = new Adhoc(_driver);
                adhoc.Deadline.Type(DateTime.Now.AddDays(15).ToString("dd-MM-yyyy"));
                if (!adhoc.OpgaverAccordion.IsExpanded) adhoc.Opgaver_adhoc_Span.Click();
                adhoc.Plus.Click();
                adhoc.Sync();

                adhoc.Opgavenavn.Type("Subtask-1");
                adhoc.Opgavenavn.SendKeys(Keys.Tab);
                header = new Header(_driver);

                var _defaultTeam = header.SelectTeam.SelectedOption;

                adhoc.Ansvarlig.WGrid_SelectByValue(_driver, _defaultTeam.GetAttribute("text"));  //Select default team.


                //04. Adding Task-2

                Assert.IsTrue(adhoc.Opgaver_Grid.Rowcount > 0, "task1 is not created");
                adhoc.Plus.Click();
                adhoc.Sync();

                adhoc.Opgavenavn.Type("Subtask-2");
                adhoc.Opgavenavn.SendKeys(Keys.Tab);

                header = new Header(_driver);

                //var _Teams = header.SelectTeam.Options;
                //var _defaultTeam2 = header.SelectTeam.SelectedOption;

                //if (_Teams.Count >= 2)
                //{
                //    foreach (var element in _Teams)
                //    {
                //        if (element.GetAttribute("text") != _defaultTeam2.GetAttribute("text"))
                //        {
                //            adhoc.Ansvarlig.WGrid_SelectByValue(_driver, element.GetAttribute("text"));  //Select next team.
                //            System.Console.WriteLine("Set Ansvarlig :" + element.GetAttribute("text"));
                //            break;
                //        }
                //    }

                //}
                //else
                //{
                //    adhoc.Ansvarlig.WGrid_SelectByValue(_driver, _defaultTeam.GetAttribute("text"), false);  //Any team other than
                //}


                var _Teams1 = adhoc.Ansvarlig1.Options(_driver);
                var _defaultTeam2 = header.SelectTeam.SelectedOption;

                if (_Teams1.Count >= 2)
                {
                    foreach (var element in _Teams1)
                    {
                        if (element.GetAttribute("textContent") != _defaultTeam2.GetAttribute("text"))
                        {
                            adhoc.Ansvarlig.WGrid_SelectByValue(_driver, element.GetAttribute("textContent"));  //Select next team.
                            Console.WriteLine("Set Ansvarlig :" + element.GetAttribute("text"));
                            break;
                        }
                    }

                }

                adhoc.Opgaver_Grid.EditBatchCell(_driver, 0, 2); //Edit cell Starterpadata before modification.
                adhoc.Starterpadato.Clear();
                adhoc.Starterpadato.Type(DateTime.Now.AddDays(1).ToString("dd-MM-yyyy"));
                adhoc.Sync();

                //05. Create the Ad-hoc Sag
                hjem.Gem.Click();
                hjem.Sync();

                toasterBar = new ToasterBar(_driver);
                string Sagnr = null;

                if (toasterBar.isDisplayed)
                {
                    Sagnr = toasterBar.ToastBar_Message.Text.Split('.')[1].Trim();
                    System.Console.WriteLine("Sagnr:" + Sagnr);
                    System.Console.WriteLine(toasterBar.ToastBar_Message.Text);
                    toasterBar.ToastBar_Ok.Click();
                    toasterBar.Sync();
                }
                else
                {
                    Assert.Fail("Toastbar is not displayed");
                }

                //6. Return Sag NO.
                return Sagnr;
            }
            catch (Exception)
            {
                return "Error during sag creation";
                closeSession();
                throw;
            }


        }

        public string GetDanID(String Lock, String DanKey)
        {

            string _key = null;

            try
            {
                var session = new Session(new ChromeDriverEnvironment());
                session.NavigateTo(ConfigurationManager.AppSettings["DanIDURL"] + DanKey);

                var _driver = session.Driver;
                var token = _driver.FindElement(By.XPath("//td[text()='" + Lock + "']"));

                if (token != null)
                {
                    var key = token.FindElement(By.XPath("./following-sibling::td[1]"));
                    Console.WriteLine("Token: " + token.GetAttribute("textContent") + ";Key: " + key.GetAttribute("textContent"));
                    _key = key.GetAttribute("textContent");
                }
                else
                {
                    Console.WriteLine("Token: " + token.ToString() + "; Key: Not present");
                }

                _driver.Close();

                return _key;

            }
            catch (Exception)
            {
                return "Token- Not present";
                throw;
            }


        }

        public void Upload2SFTP(string FileName)
        {
            var _config = ConfigurationManager.AppSettings;

            string host = _config["Host"];
            string username = _config["FTP_User"];
            string password = _config["FTP_pass"];
            string workingdirectory = _config["FTP_WDirectory"];
            string uploadfile = FilePath + FileName;

            Console.WriteLine("Creating client and connecting");
            using (var client = new Renci.SshNet.SftpClient(host, username, password))
            {
                client.Connect();
                Console.WriteLine("Connected to {0}", host);


                client.ChangeDirectory(workingdirectory);
                Console.WriteLine("Changed directory to {0}", workingdirectory);

                var listDirectory = client.ListDirectory(workingdirectory);
                Console.WriteLine("Listing directory:");
                foreach (var fi in listDirectory)
                {
                    Console.WriteLine(" - " + fi.Name);
                }


                using (var fileStream = new FileStream(uploadfile, FileMode.Open))
                {
                    Console.WriteLine("Uploading {0} ({1:N0} bytes)", uploadfile, fileStream.Length);
                    client.BufferSize = 4 * 1024; // bypass Payload error large files                    
                    client.UploadFile(fileStream, workingdirectory + FileName, null);
                }
            }


        }

        public void UpdateBill_RUC01(string FName, string EdifactDataName, string Value)
        {
            try
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + @"\Files\";

                XElement root = XElement.Load(path + FName);
                XNamespace aw = "http://scandihealth.com/praksys/integration/edigenericxml/y2014/v01";

                //Query
                IEnumerable<XElement> node =
                    from el in root.Descendants(aw + "SubElm")
                    where (string)el.Attribute("edifactDataName") == EdifactDataName.Trim()
                    select el;

                //Update
                foreach (XElement el in node)
                    el.Value = Value;

                //save file
                root.Save(path + FName);
            }

            catch (Exception e)
            {
                Console.WriteLine("UpdateBill_RUC01 :Error during update of XML. FileName : " + FName + ". Msg: " + e.Message);
                throw;
            }

        }

        public void DownloadFolder_DeleteFiles(string Filename)
        {
            /*
             * finds the local download path of the user and deletes all files matching to the given filename.
             * Note : Regular expression is used to identify the duplicates file with simillar name pattern.
             * eg: Produktion mod kladde*.pdf    
             * --will delete files such as 
             * 1. Produktion mod kladde.pdf 
             * 2. Produktion mod kladde(1).pdf
             * 3. Produktion mod kladde (2).pdf
             * 4. Produktion mod kladde locally created.pdf
             * 5. Produktion mod kladde_010121.pdf
             */

            try
            {
                string path;

                if (ConfigurationManager.AppSettings["Browser"].ToUpper().Contains("FIREFOX")) path = Path.GetTempPath();
                else path = KnownFolders.Downloads.Path + "\\"; //Chrome

                //var path = KnownFolders.Downloads.Path + "\\";

                Console.WriteLine("Local Download Path :" + path);

                var _File = Filename.Split('.');

                //Collect the files
                string[] fileArray = Directory.GetFiles(path, _File[0] + "*." + _File[1]);

                foreach (var _file in fileArray)
                {
                    Console.WriteLine("Deleted File name :" + _file);
                }

                //Delete the files
                if (fileArray.Length >= 0) Array.ForEach(fileArray, System.IO.File.Delete);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error in DeleteFiles_DownloadDirectory() method. Error message - " + e.Message);
                throw;
            }
        }

        public bool ReadPdfFile(string fileName, string searthText)
        {
            /*
            * Function check if the Text exists in the Pdf File and return  true - Yes, False - no.
            */

            try
            {
                bool TextExist = false;
                var path = KnownFolders.Downloads.Path + "\\";
                Thread.Sleep(2000); //wait for 2 seconds
                string[] fileArray = Directory.GetFiles(path, fileName);
                if (System.IO.File.Exists(fileArray[0]))
                {
                    iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(fileArray[0]);

                    for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                    {
                        iTextSharp.text.pdf.parser.ITextExtractionStrategy strategy = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                        string currentText = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
                        if (currentText.Contains(searthText))
                        {
                            TextExist = true;
                            System.Console.WriteLine("Text Exist :" + searthText);
                        }

                    }
                    pdfReader.Close();
                }
                return TextExist;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in ReadPdfFile method. Error message - " + e.Message);
                return false;
                throw;
            }
        }

        public bool DownloadFolder_FileExist(string Filename)
        {
            /*
             * Function check if the file exists in the download folder and return  true - Yes, False - no.
             * Note: 
             * 1. Overall it would wait for 2 minutes before reporting the finding. Moreover check the folder every 5 seconds during 2 mins wait time.
             * 2. It is assumed that user checks for unique filename. To achieve it User could use the DownloadFolder_DeleteFiles() prior to delete the previous instance of the file. 
             */

            try
            {
                bool FileExist = false;
                int waitTime = 0;
                string path;

                if (ConfigurationManager.AppSettings["Browser"].ToUpper().Contains("FIREFOX")) path = Path.GetTempPath();
                else path = KnownFolders.Downloads.Path + "\\"; //Chrome

                do
                {
                    waitTime++;
                    Thread.Sleep(5000); //wait for 5 seconds
                    string[] fileArray = Directory.GetFiles(path, Filename);

                    if (fileArray.Length == 1)
                    {
                        FileExist = true;
                        Console.WriteLine("File is successfully downloaded. Path :" + fileArray[0]);
                        break;
                    }
                } while (waitTime < 12);

                return FileExist;

            }
            catch (Exception e)
            {
                Console.WriteLine("Error in DownloadFolder_FileExist() method. Error message - " + e.Message);
                return false;
                throw;
            }
        }

        public bool Job_Succeed(string Jobname)
        {
            /*
             * Function check if the file exists in the download folder and return  true - Yes, False - no.
             * Note: 
             * 1. Overall it would wait for 2 minutes before reporting the finding. Moreover check the folder every 5 seconds during 2 mins wait time.
             * 2. It is assumed that user checks for unique filename. To achieve it User could use the DownloadFolder_DeleteFiles() prior to delete the previous instance of the file. 
             */

            try
            {
                bool Job_Succeed = false;
                int waitTime = 0;
                var hangfire = new Hangfire(_driver, Tabs(hangfire_Title, true));
                hangfire.Jobs.Click();
                do
                {
                    waitTime++;
                    Thread.Sleep(2000); //wait for 2 seconds                                       
                    hangfire.Processing_Jobs.Click();
                    var Processing_table = hangfire.RJ_Table; //Recurring Jobs table        
                    var columnHeader = Processing_table.FindElements(By.XPath("./thead//th"));
                    int Index_ID = columnHeader.ToList().FindIndex(Job => Job.Text.Equals("Job", StringComparison.OrdinalIgnoreCase));
                    Index_ID++;
                    var Jobs = hangfire.RJ_Table.FindElements(By.XPath("./tbody/tr/td[" + Index_ID.ToString() + "]"));
                    int Index_Job = Jobs.ToList().FindIndex(job => job.Text.Equals(Jobname, StringComparison.OrdinalIgnoreCase));

                    if (Index_Job != -1)
                    {
                        System.Console.WriteLine("Job is present in processing");
                    }

                    else
                    {
                        hangfire.Succeeded_Jobs.Click();
                        var Succeeded_table = hangfire.RJ_Table; //Recurring Jobs table  
                        var column_Header = Succeeded_table.FindElements(By.XPath("./thead//th"));
                        int index_ID = column_Header.ToList().FindIndex(Job => Job.Text.Equals("Job", StringComparison.OrdinalIgnoreCase));
                        index_ID++;
                        var Succeeded_Jobs = hangfire.RJ_Table.FindElements(By.XPath("./tbody/tr/td[" + index_ID.ToString() + "]"));
                        int Succeeded_Jobs_Index = Succeeded_Jobs.ToList().FindIndex(job => job.Text.Equals(Jobname, StringComparison.OrdinalIgnoreCase));

                        if (Succeeded_Jobs_Index != -1)
                        {
                            System.Console.WriteLine("Job is present in Succeeded Jobs");
                            Job_Succeed = true;
                            break;
                        }

                        else
                        {
                            hangfire.Failed_Jobs.Click();
                            var Failed_table = hangfire.RJ_Table; //Recurring Jobs table 
                            var Failed_column_Header = Failed_table.FindElements(By.XPath("./thead//th"));
                            int Failed_index_ID = Failed_column_Header.ToList().FindIndex(Job => Job.Text.Equals("Job", StringComparison.OrdinalIgnoreCase));
                            Failed_index_ID++;
                            var Failed_Jobs = hangfire.RJ_Table.FindElements(By.XPath("./tbody/tr/td[" + Failed_index_ID.ToString() + "]"));
                            int Failed_Jobs_Index = Failed_Jobs.ToList().FindIndex(job => job.Text.Equals(Jobname, StringComparison.OrdinalIgnoreCase));
                            Assert.IsTrue(Failed_Jobs_Index != 0, "Error");
                            break;
                        }
                    }

                } while (waitTime < 150); //wait till 5 min

                _driver.SwitchTo().DefaultContent();

                Tab.CloseTabByTitle(hangfire_Title, true);

                return Job_Succeed;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in Succeeding Jobs" + e.Message);
                return false;
                throw;
            }
        }


        public void DownloadPDF(string URL, string FileName)
        {

            try
            {
                /*  FIREFOX  --- Unfortunately 'Action' is currently not available by geckodriver 19.1
                List<Process> before = Process.GetProcessesByName("firefox").ToList();

                IWebDriver driver = new FirefoxDriver();
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(URL);

                Thread.Sleep(5000);
                //IWebElement element = driver.FindElement(By.XPath("//*[@id='pageContainer1']"));
                IWebElement element = driver.FindElement(By.XPath("//*[@id='viewerContainer']"));
                Console.WriteLine("PDF Content :" + element.GetAttribute("innerText"));
                Thread.Sleep(5000);

                Actions action = new Actions(driver);
                //action.KeyDown(Keys.Control).SendKeys("s").KeyUp(Keys.Control).Build().Perform();
                action.KeyDown(Keys.Control).SendKeys("s").KeyUp(Keys.Control).Build().Perform();

                List<Process> After = Process.GetProcessesByName("firefox").ToList();

                */


                List<Process> before = Process.GetProcessesByName("chrome").ToList();

                IWebDriver driver = new ChromeDriver();
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(URL);
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(2);

                Actions action = new Actions(driver);
                action.KeyDown(Keys.Control).SendKeys("s").KeyUp(Keys.Control).Build().Perform();

                List<Process> After;
                int synCounter = 0;

                do
                {
                    After = Process.GetProcessesByName("chrome").ToList();
                    Thread.Sleep(1000);
                    synCounter++;
                } while (before.Count == After.Count & synCounter < 10);

                List<int> before_ID = new List<int>();
                List<int> After_ID = new List<int>();

                before.ForEach(p => before_ID.Add(p.Id));
                After.ForEach(p => After_ID.Add(p.Id));

                var newProcesses = After_ID.Except(before_ID);

                foreach (var id in newProcesses)
                {
                    Console.WriteLine("************* ID -" + id + " *************");

                    Process p = Process.GetProcessById(id);
                    Console.WriteLine("Process Name:" + p.ProcessName);
                    Console.WriteLine("Main WindowTitle:" + p.MainWindowTitle);

                    if (!string.IsNullOrEmpty(p.MainWindowTitle))
                    {
                        Application myapp = Application.Attach(id);
                        Console.WriteLine("************* ID -" + id + " *************");

                        List<Window> mywindows = myapp.GetWindows();
                        foreach (var window in mywindows) Console.WriteLine("Window Name: " + window.Name);
                        Console.WriteLine("******************************************");

                        Window openDialog = mywindows.Where(n => n.Name == "Save As").First();

                        WhiteUIItem.TextBox textField = openDialog.Get<WhiteUIItem.TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndByText("File name:"));

                        textField.Text = DownloadPath + FileName;

                        WhiteUIItem.Button openButton = openDialog.Get<WhiteUIItem.Button>(SearchCriteria.ByControlType(ControlType.Button).AndByText("Save"));
                        openButton.Click();

                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error in DownloadPDF() method. Error message - " + e.Message);
                throw;
            }
        }

        public void UploadFile(string FileName, string browser)
        {

            try
            {
                List<Process> before;
                List<Process> After;

                if (browser.ToUpper().Contains("FIREFOX")) before = FF;
                else before = CH;

                After = Process.GetProcessesByName(browser).ToList();

                List<int> before_ID = new List<int>();
                List<int> After_ID = new List<int>();

                before.ForEach(p => before_ID.Add(p.Id));
                After.ForEach(p => After_ID.Add(p.Id));

                var newProcesses = After_ID.Except(before_ID);

                foreach (var id in newProcesses)
                {
                    Console.WriteLine("************* ID -" + id + " *************");

                    Process p = Process.GetProcessById(id);
                    Console.WriteLine("Process Name:" + p.ProcessName);
                    Console.WriteLine("Main WindowTitle:" + p.MainWindowTitle);

                    if (!string.IsNullOrEmpty(p.MainWindowTitle))
                    {
                        Application myapp = Application.Attach(id);
                        Console.WriteLine("************* ID -" + id + " *************");

                        List<Window> mywindows = myapp.GetWindows();
                        foreach (var window in mywindows) Console.WriteLine("Window Name: " + window.Name);
                        Console.WriteLine("******************************************");

                        Window openDialog = mywindows.Where(n => n.Name == "Open").First();

                        WhiteUIItem.TextBox textField = openDialog.Get<WhiteUIItem.TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndByText("File name:"));

                        textField.Text = FilePath + FileName;

                        //WhiteUIItem.Button openButton = openDialog.Get<WhiteUIItem.Button>(SearchCriteria.ByClassName("Button").AndByText("Open"));

                        var openButton = openDialog.Get(SearchCriteria.ByClassName("Button").AndByText("Open"));
                        Console.WriteLine("Name: " + openButton.Name);
                        openButton.Click();

                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in DownloadPDF() method. Error message - " + e.Message);
                throw;
            }
        }


    }
    [Binding]
    public class Reporting
    {

        private readonly PraksysLib praksysLib;

        public Reporting(PraksysLib _praksysLib)
        {
            if (FeatureContext.Current.ContainsKey("praksyslib"))
            {
                praksysLib = FeatureContext.Current.Get<PraksysLib>("praksyslib");
            }
            else praksysLib = _praksysLib;

        }


        [AfterStep()] // - Commented due to chrome upgrade
        public void MakeScreenshotAfterStep()
        {

            if (!praksysLib.Flag_LogOff)
            {
                var driver = praksysLib.getDriver;

                var takesScreenshot = ((ITakesScreenshot)driver);
                if (takesScreenshot != null)
                {

                    var screenshot = takesScreenshot.GetScreenshot();
                    var tempFileName = Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ".jpg";
                    var tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), tempFileName);
                    //screenshot.SaveAsFile(tempFilePath, ImageFormat.Jpeg);
                    screenshot.SaveAsFile(tempFilePath, ScreenshotImageFormat.Jpeg);

                    Console.WriteLine($"SCREENSHOT[ file:///{tempFileName} ]SCREENSHOT");

                }
            }

        }

    }



    [Binding]
    public class Reusable
    {

        private readonly PraksysLib praksysLib;

        public Reusable(PraksysLib _praksysLib)
        {
            if (FeatureContext.Current.ContainsKey("praksyslib"))
            {
                praksysLib = FeatureContext.Current.Get<PraksysLib>("praksyslib");
            }
            else praksysLib = _praksysLib;

        }

        [Then(@"close the tabs")]
        [Then(@"close the tab\.")]
        [Then(@"close the tab")]
        [Given(@"close the tab")]
        [When(@"close the tab")]
        public void ThenCloseTheTab_(Table table)
        {
            try
            {
                IEnumerable<dynamic> param = table.CreateDynamicSet();
                var tabs = praksysLib.Tab;
                foreach (var row in param)
                {
                    tabs.CloseTabByTitle(row.tab, true);
                }

            }
            catch (Exception)
            {
                praksysLib.closeSession();
                throw;
            }
        }

        [Given(@"User navigated to  '(.*)' page")]
        [Given(@"User navigates to '(.*)'")]
        public void GivenUserNavigatedToPage(string p0)
        {
            try
            {
                praksysLib.NavigateTo(p0);
            }
            catch (Exception)
            {
                praksysLib.closeSession();
                throw;
            }
        }


        [Then(@"User logs out")]
        [Then(@"User logout")]
        public void GivenUserLogsOutFromYSAndLogsIntoYA()
        {
            try
            {
                praksysLib.Logout(); ;
            }
            catch (Exception)
            {
                praksysLib.closeSession();
                throw;
            }
        }

    }

    [Binding]
    public static class Closure
    {

        [AfterFeature]
        public static void AfterFeatureRun()
        {
            Console.WriteLine("After Feature Run is invoked.");
            /*
             * To do : 
             *  1. Kill chromedriver.exe instance.
             *  2. Take into consideration about parallel run.
             * 
             */
        }
    }




    public class PraksysPages
    {

        //00. Layout & Session
        public SecurityPage securityPage { get; set; }
        public PageObjects.Core.Setup.Session session { get; set; }
        public Menu menu { get; set; }
        public ToasterBar toasterBar { get; set; }
        public Header header { get; set; }
        public Hjem hjem { get; set; }
        public string hjem_Title = "Hjem";

        //****************      01. Administration ************************************
        //01.01 Aftaler og regler
        public Aftaler aftaler { get; set; }
        public string aftaler_Title = "AFTALER";

        public LokalAftaler lokalAftaler { get; set; }
        public LokalAftaleOpret lokalaftaleopret { get; set; }

        public CreateAgreement createAgreement { get; set; }
        public string createAgreement_Title = "Lokalaftaler";
        public string LokalAftale_Opret = "Lokalaftale: Opret";
        public string LokalAftale_Name = "Lokalaftale: ";

        public AftalerDetails aftalerDetails { get; set; }
        public string aftalerKiropraktik_Title = "Aftaleområde: Kiropraktik";
        public string aftalerDetails_Title = "Aftaleområde";

        public YdelseGruppering ydelseGruppering { get; set; }
        public string ydelseGruppering_Title = "Ydelsesgrupperinger";

        public YderInfotype yderinfotype { get; set; }

        public string YderInfotype_Title = "Yder Infotype";

        public string Beskeder_Title = "Beskeder";
        public AftaleVariabler aftalevariabler { get; set; }
        public OpretYdelse opretYdelse { get; set; }
        public string opretYdelse_Title = "Ydelse: Opret";
        public string opretydelse_Title = "Opret Ydelse";
        public string ydelse_Title = "Ydelse";

        public PageObjects.UnionAndAgreements.DanReport danReport { get; set; }
        public string danReport_Title = "Ændringsrapport";

        public FindYdelser findYdelser { get; set; }
        public ReglerSet reglerset { get; set; }
        public string Reglersaet_Title = "Regelsæt: Kiropraktik";
        public string reglerset_Title = "Regelsæt: ";
        public string reglerset_title = "Regelsæt: Speciallægehjælp";
        public RegelOverview regeloverview { get; set; }
        public string regeloverview_Title = "Regelsæt";
        public Regler regler { get; set; }
        public string Regelsaet_Title = "Regelsæt";

        public RegelFamilier regelFamilier { get; set; }
        public string regelFamilier_Title = "Regelfamilier";

        public ReglerCondition reglercondition { get; set; }
        public SimulerRegelsaet simulerregelsaet { get; set; }
        public Earkiv earkiv { get; set; }

        public AftaleHistory aftaleHistory { get; set; }
        public GruppeOperations gruppeOperations { get; set; }
        public FindRegel findRegel { get; set; }



        public int rowindex;

        public string RegelsaetDetails_Title = "Regelsaet: kiropratik";
        public string Column_Aftaleomrade = "Aftaleområde";
        public string Aftaleområde_Title = "Kiropraktik";
        public string Column_Regelnavn = "Regelnavn";
        public string Regelnavn_Title = "1010 - 53 - Ydes tilskud til ydelse";
        public string Regelfamilie = "Ydes tilskud til ydelse – Kiropraktik             ";
        public string Regel_Title = "Regel:";
        public string Regel_Opret_Title = "Regelfamilie: Ydes";
        public string Regel_AEL_Title = "Regel: 53- 1045- AEL";
        public string Opret_Regel_Title = "Regel: Opret";

        public string Felt_Column = "Felt";

        public string Abonner_Column = "Abonner";
        public string Besked_Column = "Besked";

        public string Simulerregelsaet_Title = "Simuler regelsæt";



        public string Earkiv_Title = "eArkiv";
        public string Column_Dokumentnavn = "Dokumentnavn";
        public string Dokumentnavn_Title = "Simuleringsrapport til regler";


        //01.02 Økonomi
        public KontoSpecifikation kontoSpecifikation { get; set; }
        public KontoplanerDetails kontoPlanerdetails { get; set; }
        public Kontoplaner kontoplaner { get; set; }
        public Kontoplan kontoplan { get; set; }


        public string Kontoplaner_Title = "Kontoplaner";
        public string Kontoplan_Basishonorar_Title = "KontoplanBasishonorar";
        public string Opret_Kontospecifikation = "Opret Kontospecifikation";
        public string Specifikationer_column = "Specifikationer";
        public string Aftaleomrade_Title = "Fodterapi";
        public string Status_column = "Status";
        public string Status = "Godkendt";
        public string Title_Status = "Sendt til godkendelse";
        public string Title_Afvis = "Afvist";
        public string Title_KladdeStatus = "Kladde";
        public int rowindexValue;
        public string column_gyldigfra = "Gyldig fra";
        public string column_Handling = "Handling";



        public string Kontoplan_Partial_Title = "Kontoplan"; //Kontoplan Basishonorar
        public string Kontoplan_Titles = "Kontogruppe:";
        public string Column_Kontonummer = "Kontonummer";
        //01.03 Claims

        public SearchUdbetalingsgrundlag searchUdbetalingsgrundlag { get; set; }
        public string searchUdbetalingsgrundlag_Title = "Udbetalingsgrundlag";


        public RegTideling regTideling { get; set; }
        public string regTideling_Title = "Regningstildeling";

        public Forretningsregler forretningsregler { get; set; }
        public string forretningsregler_Title = "Forretningsregler";

        //01.04 Yder
        public ProviderCategoryPopup providerCategoryPopup { get; set; }
        public SearchYder searchYder { get; set; }
        public Faellesskab faellesskab { get; set; }
        public string faellesskab_Title = "Fællesskab";
        public string searchYder_Title = "Yder";
        public string searchYder_Column = "Ydernr.";
        public string Yder_Title = "Yder: ";
        public string cockpit_Title = "Cockpit:";
        public YderBasicDetail yderBasicDetail { get; set; }
        public Yderperson yderperson { get; set; }
        public Hændelsesoversigt hændelsesoversigt { get; set; }
        public string hændelsesoversigt_title = "Hændelsesoversigt";
        public YderInfotype yderInfotype { get; set; }
        public string yderInfotype_Title = "Yder Infotype";
        public PageObjects.Provider.Provider provider { get; set; }
        public string provider_Title = "Opret Yder";
        public Lokation lokation { get; set; }
        public Opretnypopup opretnypopup { get; set; }
        //public Faellesskab faellesskab { get; set; }
        public SearchFaellesskab searchfaellesskab { get; set; }
        public string searchFaellesskab_title = "Fællesskab";
        public SimpelKlassifikation simpelKlassifikation { get; set; }
        public string simpelKlassifikation_Title = "Klassifikationer";
        public Stamoplysninger stamoplysninger { get; set; }
        public YderCockpit ydercockpit { get; set; }
        public string _scopeid = "";
        public int Provider1Value;
        public string Title_Cockpitdetail;
        public string Title_Yderdetail;



        public string Sag_Notification_Title = "Sag: ";

        public AOY_ProviderSignUp AOY { get; set; }

        public YderHistorik yderhistorik { get; set; }
        public string yderhistorik_Title = "Historik for yder";


        public ReguleringerDetails reguleringerDetails { get; set; }
        public string reguleringerDetails_Title = "Regulering detaljer";


        //01.05 Klassifikation

        //01.06 eArkiv

        public EArchive eArchive { get; set; }
        public string eArchive_Title = "eArkiv";

        //01.07 Konfiguration           
        public Myndighed myndighed { get; set; }
        public string myndighed_Title = "Myndighed";

        public Hændelser hændelser { get; set; }
        public string hændelser_title = "Håndter hændelse";

        public Dokumenttyper dokumenttyper { get; set; }
        public string dokumenttyper_title = "Dokumenttyper";

        public Sager sager { get; set; }
        public string sager_title = "Sager";

        public Bruger bruger { get; set; }
        public string bruger_Title = "Bruger";

        public AdresseoplysningerPopup adresseoplysningerPopup { get; set; }
        public Adresseoplysninger adresseoplysninger { get; set; }
        public Rolle rolle { get; set; }
        public string rolle_Title = "Rolle";
        public TildelOpgave tildelopgave { get; set; }
        public string tildelopgave_Title = "Tildel opgave til funktionel rolle";
        public TildelRettighed tildelrettighed { get; set; }
        public string tildelrettighed_Title = "Tildel rettighed til funktionel rolle";
        public Daekningsomrade daekningsomrade { get; set; }
        public string daekningsomrade_Title = "Dækningsområde";

        public TidelMyndighed_TilDaekningsomrade tidelMyndighed_tilDaekningsomrade { get; set; }
        public string tidelMyndighed_tilDaekningsomrade_Title = "Tildel myndighed til dækningsområde";
        public Myndighedsansvar myndighedsansvar { get; set; }
        public string myndighedsansvar_Title = "Myndighedsansvar";
        public KombinerAnsvar kombineransvar { get; set; }
        public string kombineransvar_Title = "Kombiner ansvar";
        public Team team { get; set; }
        public string team_Title = "Team";
        public TildelBrugerTilTeam tildelbrugerTilTeam { get; set; }
        public string tildelbrugerTilTeam_Title = "Tildel bruger til team";
        public TildelMyndighedsansvar tildelmyndighedsansvar { get; set; }
        public string tildelmyndighedsansvar_Title = "Tildel myndighedsansvar";
        public Beskeder beskeder { get; set; }
        public Adhoc adhoc { get; set; }
        public string adhoc_Title = "Ad hoc-sag";
        //public string Title_Adhocsag = "Ad hoc-sag";
        public Sag sag { get; set; }
        public string Sag_Title;
        public SagDetails sagDetails { get; set; }



        //01.08 Udbateling

        public Godkend godkend { get; set; }
        public UdbUdvidet udbUdvidet { get; set; }
        public Udbetalinger udbetalinger { get; set; }

        public Udbetaling udbetaling { get; set; }

        public string Godkend_Title = "Godkend udbetaling";
        public string Aftaleomrade_column = "Aftaleområde";
        public string colmn_text = "Almen Praksis";
        public string AftaleomradeColumntext_Fysioterapi = "Fysioterapi";
        public string Udbatelinger_Title = "Udbetalinger";
        public string Navn_Column = "Navn";
        public string Navn_Columntext = "";
        public string UdbetalingUdvidet_Title = "Udbetaling udvidet ";
        public string Status_Payment = "Status";
        public string Status_Bestilt = "Bestilt";
        public string Status_Column = "Status";
        public string Status_Columntext = "Fejl";
        public string Nemkonto_Status = "NemKonto status";
        public string NemkontoColumn_Text = "I gang";
        public string Udbateling_Title = "Udbetaling:";
        public string EArkiv_Title = "eArkiv";
        public string Dokumenttype_Column = "Dokumenttype";
        public string Dokumenttype_Text = "AfrUdb001D Honorarspecifikation til yder";


        //01.09 SelfService
        public SelfService selfService { get; set; }
        public PageObjects.ProviderSelfService.Headerselfservice headerselfservice { get; set; }
        public PageObjects.ProviderSelfService.Menuselfservice menuselfservice { get; set; }
        public PageObjects.ProviderSelfService.Providerselfservice providerselfservice { get; set; }
        public PageObjects.ProviderSelfService.Vikarudbetaling vikarudbetaling { get; set; }
        public PageObjects.ProviderSelfService.Afmeld afmeld { get; set; }
        public PageObjects.ProviderSelfService.GENERELT generelt { get; set; }

        public string nogleValue;

        public SelfServiceHome selfServiceHome { get; set; }

        public OpretRegning opretRegning { get; set; }




        //01.10 Noglekort
        //public Noglekort noglekort { get; set; }



        //****************      02. Sys Administration  ************************************

        public Hangfire hangfire { get; set; }
        public string hangfire_Title = "Hangfire";

        public RabbitMQ rabbitMQ { get; set; }
        public string rabbitMQ_Title = "RabbitMQ";

        public OIP oip { get; set; }
        public string oip_Title = "OIP";

        public Ecrion ecrion { get; set; }
        public string ecrion_Title = "Ecrion";

        //****************      03. Regningsbehandling  ************************************
        public Regningsoverblik regningsoverblik { get; set; }
        public string regningsoverblik_Title = "Regningsoverblik";


        public SearchBundter searchBundter { get; set; }
        public string searchBundter_Title = "Bundter";

        public SearchRegninger searchRegninger { get; set; }
        public string searchRegninger_Title = "Regninger";


        public Henvisninger henvisninger { get; set; }
        public string Henvisninger_title = "Henvisninger";

        public Reguleringer reguleringer { get; set; }
        public string reguleringer_Title = "Reguleringer";


        //****************      04. Afregning           ************************************


        //****************      05. Udbetaling          ************************************


        //****************      06. Opfølgning          ************************************


        //****************      07. Borger              ************************************





    }

    class CreateProvider
    {

        IWebDriver _driver;

        public CreateProvider(IWebDriver driver)
        {
            _driver = driver;
        }


    }
}

