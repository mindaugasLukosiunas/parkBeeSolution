using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace ParkBeeSulution.UITests
{
    class ParkBeeHomePageElements
    {
        IWebDriver _driver;
        WebDriverWait _wait;

        private string id_locationInput = "directions__start";
        private string name_date = "date";
        private string xpath_timeConfirmationButton = "//div[@class='timepicker-confirm']//a";      
        private string xpath_searchButon = "//button[contains(.,'Zoeken')]";
        
        public ParkBeeHomePageElements(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        public void SelectLocation(string location)
        {
            var element = _driver.FindElement(By.Id(id_locationInput));        
            element.Clear();
            element.SendKeys(location);
        }

        public void SelectCurrentTime()
        {
            var dateInputElement = _driver.FindElement(By.Name(name_date));
            dateInputElement.Click();

            var timeConfirmationButtonElement = _driver.FindElement(By.XPath(xpath_timeConfirmationButton));
            timeConfirmationButtonElement.Click();
        }

        public void ClickSearchButton()
        {
            var element = _driver.FindElement(By.XPath(xpath_searchButon));
            element.Click();
        }        
    }
}
