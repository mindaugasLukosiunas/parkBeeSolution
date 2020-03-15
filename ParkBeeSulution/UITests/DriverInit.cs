using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkBeeSulution.UITests
{
    public class DriverInit
    {
        IWebDriver _driver;

        private string baseUrl = "https://parkbee.com/nl";

        public IWebDriver GetDriver()
        {
            InitiateDriver();
            return _driver;
        }

        public void InitiateDriver()
        {
            _driver = new ChromeDriver();
            _driver.Url = baseUrl;
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            _driver.Manage().Window.Maximize();
        }
    }
}
