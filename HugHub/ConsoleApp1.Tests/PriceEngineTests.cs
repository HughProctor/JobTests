using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ConsoleApp1.Tests
{
    [TestClass]
    public class PriceEngineTests
    {
        [TestMethod]
        public void First_Refactor_Test()
        {
            var request = new PriceRequest()
            {
                RiskData = new RiskData() //hardcoded here, but would normally be from user input above
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

        }
    }
}
