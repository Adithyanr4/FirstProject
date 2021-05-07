using System;
using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace AutoProject
{
    [TestFixture]
    public class Test
    {
        IWebDriver driver;
        private static string browser;
        IWebElement searchbox, searchbutton;


        [OneTimeSetUp]
        public void StartBrowser()
        {
            browser = ConfigurationManager.AppSettings["browser"];
            switch (browser)
            {
                case "chrome":
                    driver = new ChromeDriver();
                    break;
                case "firefox":
                    driver = new FirefoxDriver();
                    break;
                default:
                    break;
            }
            
            driver.Url = @"https://codility-frontend-prod.s3.amazonaws.com/media/task_static/qa_csharp_search/862b0faa506b8487c25a3384cfde8af4/static/attachments/reference_page.html";
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
        }

        [TestCase]
        public void VerifySearchBoxAndButton()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            searchbox = FindElement("search-input", 60);
            searchbutton = FindElement("search-button", 60);
            Assert.NotNull(searchbox, "Failed to find searchbox");
            Assert.NotNull(searchbutton, "Failed to find searchbtn");
            
             }

        [TestCase]
        public void VerifyEmptyTextSearch()
        {
            searchbox.SendKeys("");
            searchbutton.Click();
            IWebElement emptyquerytextEle = FindElement("error-empty-query", 60);
            Assert.NotNull(emptyquerytextEle, "Failed to display empty query message");
            Assert.AreEqual("Provide some query", emptyquerytextEle.Text, "Failed to display empty query message");

        }

        [TestCase]
        public void VerifyIslandSearch()
        {
            searchbox.SendKeys("isla");
            searchbutton.Click();
            IWebElement resultEle = FindElement("search - results", 60);
            IList<IWebElement> resultList = resultEle.FindElements(By.XPath("//li"));
            Assert.IsTrue(resultList.Count > 0, "Failed to fetch results");
        }

        [TestCase]
        public void VerifyWrongTextSearch()
        {
            searchbox.SendKeys("castle");
            searchbutton.Click();
            IWebElement noResultEle = FindElement("error-no-results", 60);
            Assert.NotNull(noResultEle, "Failed to display no result message");

            }

        [TestCase]
        public void VerifyPortSearch()
        {
            
            searchbox.SendKeys("port");
            searchbutton.Click();
            IWebElement portresultEle = FindElement("search - results", 60);
            IList<IWebElement> portresultList = portresultEle.FindElements(By.XPath("//li"));
            Assert.IsTrue(portresultList.Count == 1, "Failed to fetch results");
            Assert.IsTrue(portresultList[0].Text == "Port Royal", "Failed to fetch results");
        }

        public IWebElement FindElement(string ID,int timespan)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timespan));
                return wait.Until(ExpectedConditions.ElementExists(By.Id(ID)));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private IWebElement Func(IWebDriver webDriver, IWebDriver driver, int v, IWebDriver iD)
        {
            throw new NotImplementedException();
        }

        [OneTimeTearDown]
        public void CloseBrowser()
        {
            driver.Quit();
        }
    }
}
