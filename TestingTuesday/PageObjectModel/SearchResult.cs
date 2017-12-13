using OpenQA.Selenium;
using PageObjectModel.Controls;
using Praksys.PageObjects.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjectModel
{
    public class SearchResult:PageBase

    {
        private IWebElement _pagecontent = null;

        public SearchResult(IWebDriver webdriver)
            : base(webdriver)
        {

        }

        public Tab DelarsTabs
        {
            get
            {
                return new Tab(FindElement(By.XPath("//*[@id='srpTabs']/ul[contains(@class,'dealer-private-tabs')]//li")));
            }
        }


    }
}
