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
    public class Bilbasen:PageBase

    {

        private IWebElement _pagecontent = null;

        public Bilbasen(IWebDriver webdriver)
            : base(webdriver)
        {
              
        }

        public Dropdown Kategori
        {
            get
            {
                return new Dropdown(FindElement(By.XPath("//*[@id='inline-search-collapse']//label[text()='Kategori']/..//select")));
            }
        }


        public Dropdown Mærke
        {
            get
            {
                return new Dropdown(FindElement(By.XPath("//*[@id='inline-search-collapse']//label[text()='Mærke']/..//select")));
            }
        }

        public Dropdown Model
        {
            get
            {
                return new Dropdown(FindElement(By.XPath("//*[@id='inline-search-collapse']//label[text()='Model']/..//select")));
            }
        }


        public Dropdown Brændstof
        {
            get
            {
                return new Dropdown(FindElement(By.XPath("//*[@id='inline-search-collapse']//label[text()='Brændstof']/..//select")));
            }
        }

        public Dropdown PrisFra
        {
            get
            {
                return new Dropdown(FindElement(By.XPath("//*[@id='inline-search-collapse']//label[text()='Pris']/..//select/option[text()='fra']/..")));
            }
        }


        public Dropdown PrisTil
        {
            get
            {
                return new Dropdown(FindElement(By.XPath("//*[@id='inline-search-collapse']//label[text()='Pris']/..//select/option[text()='til']/..")));
            }
        }

        public Dropdown ÅrFra
        {
            get
            {
                return new Dropdown(FindElement(By.XPath("//*[@id='inline-search-collapse']//label[text()='År']/..//select/option[text()='fra']/..")));
            }
        }

        public Dropdown ÅrTil
        {
            get
            {
                return new Dropdown(FindElement(By.XPath("//*[@id='inline-search-collapse']//label[text()='År']/..//select/option[text()='til']/..")));
            }
        }

        public Dropdown KmFra
        {
            get
            {
                return new Dropdown(FindElement(By.XPath("//*[@id='inline-search-collapse']//label[text()='Km']/..//select/option[text()='fra']/..")));
            }
        }

        public Dropdown KmTil
        {
            get
            {
                return new Dropdown(FindElement(By.XPath("//*[@id='inline-search-collapse']//label[text()='Km']/..//select/option[text()='til']/..")));
            }
        }


        public Button VisKunLeasingBiler
        {
            get
            {
                return new Button(FindElement(By.XPath("//button[text()='Vis kun leasingbiler']")));
            }
        }

        public Button VisBiler
        {
            get
            {
                return new Button(FindElement(By.XPath("//button[@id='usedcaradjustsubmit']")));
            }
        }

        public Checkbox NyeBiler
        {
            get
            {
                return new Checkbox(FindElement(By.XPath("//span[text()='Nye biler']/../input")));
            }
        }

        public Checkbox EngrosCVR
        {
            get
            {
                return new Checkbox(FindElement(By.XPath("//span[text()='Engros/CVR']/../input")));
            }
        }

        public Checkbox BrugteBiler
        {
            get
            {
                return new Checkbox(FindElement(By.XPath("//span[text()='Brugte biler']/../input")));
            }
        }


        public Checkbox Leasing
        {
            get
            {
                return new Checkbox(FindElement(By.XPath("//span[text()='Leasing']/../input")));
            }
        }

        public TextBox Search
        {
            get
            {
                return new TextBox(FindElement(By.XPath("//button[contains(@class,'header-free-search-button')]")));
            }
        }


    }
}
