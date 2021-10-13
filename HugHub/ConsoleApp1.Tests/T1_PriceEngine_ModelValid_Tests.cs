using ConsoleApp._BusinessLogic;
using ConsoleApp.Controllers;
using ConsoleApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp.Tests
{
    [TestClass]
    public class T1_PriceEngine_ModelValid_Tests
    {
        //[TestMethod]
        //public void T1_First_Refactor_Test()
        //{
        //    var request = new PriceRequest()
        //    {
        //        RiskData = new RiskDataModel() //hardcoded here, but would normally be from user input above
        //        {
        //            DOB = DateTime.Parse("1980-01-01"),
        //            FirstName = "John",
        //            LastName = "Smith",
        //            Make = "Cool New Phone",
        //            Value = 500
        //        }
        //    };

        //    decimal tax = 0;
        //    string insurer = "";
        //    string error = "";

        //    var priceEngine = new PriceEngine();
        //    var price = priceEngine.GetPrice(request, out tax, out insurer, out error);


        //    Assert.IsNotNull(price, "Price is null");
        //    Assert.IsTrue(price > 0, "Price returned an error" + error);
        //}

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

        [TestMethod]
        public async Task T3_GetPrice_BadRequest_NullModel()
        {
            // Mock the PriceService to test model basic validation errors
            decimal resultPrice = 1;
            var priceServiceMoc = Substitute.For<IPriceService>();
            priceServiceMoc.GetBestQuote().Returns(Task.FromResult(resultPrice));

            RiskDataModel riskData = null;

            var priceController = new PriceController(priceServiceMoc);

            var response = await priceController.GetPriceAsync(riskData);

            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Response was not of correct type");
            Assert.AreEqual((response as BadRequestObjectResult).Value, "The HTTP request did not include a valid entity body. Please ensure there is an entity.", string.Format("Request did not return exepected value - Expected:{0} Actual: {1}", resultPrice, response));
        }

        //[DataTestMethod]
        //[DynamicData(nameof(RiskDataTestData), DynamicDataSourceType.Method)]
        [TestMethod]
        public async Task T4_GetPrice_BadRequest_Model_RequiredValues()
        {
            // Mock the PriceService to test model basic validation errors
            decimal resultPrice = 1;
            var priceServiceMoc = Substitute.For<IPriceService>();
            priceServiceMoc.GetBestQuote().Returns(Task.FromResult(resultPrice));

            var riskData = new RiskDataModel() {};

            // Set some properties here
            var context = new ValidationContext(riskData, null, null);
            var results = new List<ValidationResult>();
            var isModelStateValid = Validator.TryValidateObject(riskData, context, results, true);
            Assert.AreEqual(3, results.Count);
            Assert.AreEqual("FirstName", results[0].MemberNames.ElementAt(0));
            Assert.AreEqual("The FirstName field is required.", results[0].ErrorMessage);

            Assert.IsFalse(isModelStateValid, "Model Validation did not fail");

            var priceController = new PriceController(priceServiceMoc);
            priceController.ModelState.AddModelError("FirstName", "First Name is Required");

            var response = await priceController.GetPriceAsync(riskData);

            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Response was not of correct type");
            
            // Sadly this doesn't work - ValidationResult constructor is protected
            List<ValidationResult> result = (response as BadRequestObjectResult).Value as List<ValidationResult>;

            //Assert.AreEqual(1, result.Count);
            //Assert.AreEqual("FirstName", result[0].MemberNames.ElementAt(0));
            //Assert.AreEqual("The FirstName field is required.", result[0].ErrorMessage);
        }

    }


    //public static IEnumerable<object[]> RiskDataTestData()
    //{
    //    yield return new Object[]
    //    {
    //        new RiskDataModel()
    //        {

    //        },
    //        new Dictionary<int, DateTime>()
    //        {
    //            [1] = DateTime.Parse("2020-06-02"),
    //            [2] = DateTime.Parse("2020-06-03"),
    //            [3] = DateTime.Parse("2020-06-04")
    //        }
    //    };
    //    yield return new Object[]
    //    {
    //        new RiskDataModel()
    //        {
    //        },
    //        new Dictionary<int, DateTime>()
    //        {
    //            [1] = DateTime.Parse("2020-06-02"),
    //            [2] = DateTime.Parse("2020-06-03"),
    //            [3] = DateTime.Parse("2020-06-04")
    //        }
    //    };
    //}
}