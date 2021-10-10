using ConsoleApp1.Tests.TestMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Tests
{
    [TestClass]
    public class T2_ParallelTask_Tests
    {
        [TestMethod]
        public async Task T1_ParallelTask_Test()
        {
            var bestQuote = await new ParallelTasks().SumPageSizesAsync();

            Assert.IsNotNull(bestQuote, "Best Quote returned null");
        }
    }
}
