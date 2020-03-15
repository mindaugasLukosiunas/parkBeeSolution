using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace ParkBeeSulution.UITests
{
    class Tests
    {
        private IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            _driver = new DriverInit().GetDriver();
        }

        /*This test actually covers both scenarios, it checks if prices are shown after initial search,
        updates the time and checks again if those prices were updated*/
        [Test]
        public void ShowPricesForSearchResults()
        {
            var homePage = new ParkBeeHomePageElements(_driver);
            homePage.SelectLocation("Nassaukade, Amsterdam, Netherlands");
            homePage.SelectCurrentTime();
            homePage.ClickSearchButton();

            var searchResultsPage = new GarageMapPageElements(_driver);
            var pricesOnMapBeforeChangingTime = searchResultsPage.GetAllVisiblePricesOnMap();
            pricesOnMapBeforeChangingTime.Should().NotBeNullOrEmpty();

            searchResultsPage.SelectParkingEndDate("08");

            homePage.ClickSearchButton();

            var pricesOnMapAfter = searchResultsPage.GetAllVisiblePricesOnMap();
            pricesOnMapAfter.Should().NotBeNullOrEmpty();
            pricesOnMapBeforeChangingTime.Should().NotBeEquivalentTo(pricesOnMapAfter);
            pricesOnMapAfter.Should().NotContain(pricesOnMapBeforeChangingTime);
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }
    }
}
