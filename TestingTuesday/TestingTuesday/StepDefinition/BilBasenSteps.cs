using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using PageObjectModel;
using System;
using System.Configuration;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace TestingTuesday.StepDefinition
{
    [Binding]
    public class BilBasenSteps
    {        
        IWebDriver driver; 
        private readonly Lib lib;

        public BilBasenSteps(Lib _lib)
        {
            if (FeatureContext.Current.ContainsKey("lib"))
            {
                lib = FeatureContext.Current.Get<Lib>("lib");
            }
            else lib = _lib;
        }

        [Given(@"User is in Bilbasen page")]
        public void GivenUserIsInBilbasenPage()
        {

            try
            {
                lib.StartSession();
                driver = lib.driver;
                driver.Navigate().GoToUrl(ConfigurationManager.AppSettings["URL"]);
            }
            catch (Exception)
            {
                lib.closeSession();
                throw;
            }

        }

        [When(@"searches for car older than (.*)")]
        public void WhenSearchesForCarOlderThan(int p0)
        {
            try
            {
                lib.bilbasen = new Bilbasen(driver);
                lib.bilbasen.ÅrTil.SelectByText(p0.ToString());
                lib.bilbasen.Search.Click();
            }
            catch (Exception)
            {
                lib.closeSession();
                throw;
            }
        }

        [Then(@"bilbasen result private and dealer cars listed for the same")]
        public void ThenBilbasenResultPrivateAndDealerCarsListedForTheSame()
        {
            try
            {
                lib.searchResult = new SearchResult(driver);
                Assert.IsTrue(lib.searchResult.DelarsTabs.Displayed);
            }
            catch (Exception)
            {
                lib.closeSession();
                throw;               
            }
        }


        [When(@"user search for car")]
        public void WhenUserSearchForCar(Table table)
        {
            try
            {
                dynamic param = table.CreateDynamicInstance();

                var bilbasen = lib.bilbasen;
                bilbasen = new Bilbasen(driver);
                bilbasen.Mærke.SelectByText(param.Mærke);
                bilbasen.KmFra.SelectByText(param.km_Fra);
                bilbasen.KmTil.SelectByText(param.km_till);
                bilbasen.ÅrFra.SelectByText(Convert.ToString((int)param.År_fra));
                //bilbasen.ÅrFra.SelectByText(param.År_fra);
                bilbasen.Search.Click();
            }
            catch (Exception)
            {
                lib.closeSession();
                throw;
            }            
        }

    }
}
