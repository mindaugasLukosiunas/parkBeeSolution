using RestSharp;
using System;
using Newtonsoft.Json;
using NUnit.Framework;
using FluentAssertions;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ParkBeeSulution.APITests
{
    class CalculatePostTests
    {
        RestClient _client;
        /*The test is only for price calculation endpoint, 
         * therefore some data is hard-coded that I got from other endpoints with postman.
         * Of course getting this data should be done dynamically */
        string mainURL = "https://api-uat.parkbee.net";
        string endpoint = "/v1/garages/{0}/pricing/calculate";
        //You need to add the accessToken in order to authorize
        string accessToken = "";
        string garageID = "afce8e5b-7fa7-4f1e-bb98-cfb9ce00ad1b";
             
        [SetUp]
        public void SetUp()
        {
            _client = new RestClient(mainURL);
        }

        [TestCase("2020-09-26Z12:00:00", "2020-09-26Z12:00:00")]
        [TestCase("2020-09-26Z12:00:00", "2020-10-30Z12:00:00")]
        public void ValidRequestReturnsSuccessResponseCode(string startTime, string endTime)
        {
            var request = FormatCalculatePriceRequest(endpoint, garageID, startTime, endTime, accessToken);
            var response = _client.Post(request);

            response.IsSuccessful.Should().BeTrue();
        }

        [TestCase("2020-09-26Z12:00:00", "2020-10-30Z25:00:00")]
        [TestCase("0000-00-00 00:00:00", "2020-09-27 12:00:00")]
        [TestCase("2020-09-26T12:00:00UTC", "2020-09-26T12:00:00UTC")]
        [TestCase("2020-09-26", "2010-10-30")]
        [TestCase("2020-09-26T12:00:00", "2010-10-30T12:00:00")]
        [TestCase("2020-09-26Z12:00:00", "2010-10-30Z12:00:00")]
        [TestCase("", "")]
        [TestCase(null, null)]
        public void InvalidRequestReturnsBadRequest(string startTime, string endTime)
        {
            var request = FormatCalculatePriceRequest(endpoint, garageID, startTime, endTime, accessToken);
            var response = _client.Post(request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Content.Should().Contain("errorMessage");
        }
     
        [TestCase("2020-03-18Z08:00:00", "2020-03-18Z08:30:00", "")]
        [TestCase("2020-03-18Z08:00:00", "2020-03-18Z08:30:00", null)]
        public void ReturnsNotFoundWithInvalidGarageID(string startTime, string endTime,string garage)
        {
            var request = FormatCalculatePriceRequest(endpoint, garage, startTime, endTime, accessToken);
            var response = _client.Post(request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            response.Content.Should().BeNullOrEmpty();
        }

        [TestCase("2020-09-26 12:00:00", "2020-09-27 12:00:00")]
        [TestCase("2020-02-26Z12:00:00", "2023-02-28Z12:00:00")]
        [TestCase("2020-02-26Z12:00:00", "2020-02-28Z12:00:00")]
        [TestCase("2020-03-15Z12:00:00", "2020-03-15Z13:00:00")]
        [TestCase("2020-03-15Z12:00:00", "2020-03-15Z12:00:01")]
        [TestCase("2020-03-18Z08:00:00", "2020-03-18Z08:30:00")]
        [TestCase("2020-03-15Z12:00:00", "2020-03-15Z12:00:00")]
        [TestCase("2020-03-15Z12:00:00", "2100-03-15Z12:00:00")]
        /*The last one fails, the endpoint does not handle ridiculous dates in the future, 
         * returns a negative number. Well, theoretically it calculates, but maybe consider
         * an error message for such future dates */
        [TestCase("2020-03-15Z12:00:00", "9999-03-15Z12:00:00")]
        public void PriceCalculatedWithValidData(string startTime, string endTime)
        {
            var request = FormatCalculatePriceRequest(endpoint, garageID, startTime, endTime, accessToken);
            var response = _client.Post(request);

            var costJson = JObject.Parse(response.Content);

            costJson.Should().NotBeNull();
            costJson.Should().NotBeEmpty();
            costJson.Should().HaveCount(1);

            /*Even though a float type is returned in json, I take the value as an int to be able to do comparison with zero.
            Every case returned a number that could be parsed to int*/
            var price = costJson.Value<int>("cost");
            
            price.Should().BeGreaterOrEqualTo(0);
        }

        public RestRequest FormatCalculatePriceRequest(string endpoint, string garageID, string start, string end, string token)
        {           
            var request = new RestRequest(String.Format(endpoint, garageID), Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);

            var json_data = new
            {
                startTime = start,
                endTime = end
            };

            string json = JsonConvert.SerializeObject(json_data);
           
            request.AddJsonBody(json);

            return request;
        }
    }
}
