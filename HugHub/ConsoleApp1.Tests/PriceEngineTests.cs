using ConsoleApp._BusinessLogic;
using ConsoleApp.Controllers;
using ConsoleApp1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace ConsoleApp1.Tests
{
    [TestClass]
    public class PriceEngineTests
    {
        [TestMethod]
        public void T1_First_Refactor_Test()
        {
            var request = new PriceRequest()
            {
                RiskData = new RiskDataModel() //hardcoded here, but would normally be from user input above
                {
                    DOB = DateTime.Parse("1980-01-01"),
                    FirstName = "John",
                    LastName = "Smith",
                    Make = "Cool New Phone",
                    Value = 500
                }
            };

            decimal tax = 0;
            string insurer = "";
            string error = "";

            var priceEngine = new PriceEngine();
            var price = priceEngine.GetPrice(request, out tax, out insurer, out error);


            Assert.IsNotNull(price, "Price is null");
            Assert.IsTrue(price > 0, "Price returned an error" + error);
        }

        [TestMethod]
        public async Task T2_Refactor_Split_GetPriceModelAsync()
        {
            // Mock the PriceService to test model basic validation errors
            decimal resultPrice = 1;
            var priceServiceMoc = Substitute.For<IPriceService>();
            priceServiceMoc.GetBestQuote().Returns(Task.FromResult(resultPrice));

            var riskData = new RiskDataModel() 
            {
                DOB = DateTime.Parse("1980-01-01"),
                FirstName = "John",
                LastName = "Smith",
                Make = "Cool New Phone",
                Value = 500
            };

            var priceController = new PriceController(priceServiceMoc);

            var response = await priceController.GetPriceAsync(riskData);

            Assert.IsInstanceOfType(response, typeof(OkObjectResult), "Response was not of correct type");
            Assert.AreEqual((response as OkObjectResult).Value, resultPrice, string.Format("Request did not return exepected value - Expected:{0} Actual: {1}", resultPrice, response));
        }
    }
}
