using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkBeeSulution.UITests
{
    class GarageMapPageElements
    {
        IWebDriver _driver;
        WebDriverWait _wait;

        private string name_parkingEndDate = "dateEnd";
        private string xpath_Day = "//td[contains(@class, 'day')]";
        private string xpath_Hour = "//td[contains(@class, 'hour')]";
        private string xpath_timePickerHour = "//span[@class='timepicker-hour']";    
        private string xpath_timeConfirmationButton = "//div[@class='timepicker-confirm']//a";
        private string xpath_pricesOnMap = "//span[@class='custom-marker__amount']";
        private string xpath_priceLoader = "//span[@class='custom-marker__loading pb__loader']";
        private string xpath_defaultIcon = "//span[@class='custom-marker__default-icon']";

        public GarageMapPageElements(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        public List<string> GetAllVisiblePricesOnMap()
        {    
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath(xpath_defaultIcon)));
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath(xpath_priceLoader)));

            var elements = _driver.FindElements(By.XPath(xpath_pricesOnMap));

            var priceList = new List<string>();

            foreach (IWebElement priceElement in elements)
            {
                if(!String.IsNullOrEmpty(priceElement.Text))
                priceList.Add(priceElement.Text);
            }

            return priceList;
        }

        public void SelectParkingEndDate(string hour, string activeDay = "")
        {
            SelectDay(activeDay);
            SelectHour(hour);    
            ClickConfirmTimeButton();
        }

        private void SelectDay(string activeDay = "")
        {
            var endDateInput = _driver.FindElement(By.Name(name_parkingEndDate));
            endDateInput.Click();

            if (activeDay.Equals(""))
            {
                activeDay = DateTime.Today.AddDays(1).Day.ToString();
            }

            var activeDays = _driver.FindElements(By.XPath(xpath_Day));

            if (activeDays.Count() > 0)
            {
                activeDays.Where(e => e.Text.Equals(activeDay))
                .FirstOrDefault()
                .Click();
            }
        }

        private void SelectHour(string hour)
        {
            _driver.FindElement(By.XPath(xpath_timePickerHour))
            .Click();

            var hours = _driver.FindElements(By.XPath(xpath_Hour));

            if (hours.Count() > 0)
            {
                hours.Where(e => e.Text.Equals(hour))
                .FirstOrDefault()
                .Click();
            }
        }

        private void ClickConfirmTimeButton()
        {
            var timeConfirmationButtonElement = _driver.FindElement(By.XPath(xpath_timeConfirmationButton));
            timeConfirmationButtonElement.Click();
        }
    }
}