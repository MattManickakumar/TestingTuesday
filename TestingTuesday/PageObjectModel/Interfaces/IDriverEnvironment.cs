using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace PageObjectModel
{
    public interface IDriverEnvironment
    {

       IWebDriver CreateWebDriver();
       string DriverPath { get; set; }

    }
}
