using ConsoleApp;
using ConsoleApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Tests
{
    [TestClass]
    public class T3_PriceEngineService_Tests
    {
        [TestMethod]
        public async Task Refactored_PriceEngine_Test()
        {
            var service = new PriceEngineService();

            var riskData = new RiskDataModel()
            {
                DOB = DateTime.Parse("1980-01-01"),
                FirstName = "John",
                LastName = "Smith",
                Make = "Cool New Phone",
                Value = 500
            };

            var bestQuote = await service.GetBestPrice(riskData);

        }
    }
}
