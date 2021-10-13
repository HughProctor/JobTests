using ConsoleApp;
using ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program2
    {
        static Task Main() => RunProgram();

        static async Task RunProgram()
        {
            //SNIP - collect input (risk data from the user)

            var request = new PriceRequest()
            {
                RiskData = new RiskDataModel()
                {
                    DOB = DateTime.Parse("1980-01-01"),
                    FirstName = "John",
                    LastName = "Smith",
                    Make = "Cool New Phone",
                    Value = 500
                }
            };
            var service = new PriceEngineService();
            var bestQuote = await service.GetBestPrice(request.RiskData);

            //if (bestQuote  == -1)
            //{
            //    Console.WriteLine(String.Format("There was an error - {0}", error));
            //}
            //else
            //{
                Console.WriteLine(String.Format("You price is {0}, from insurer: {1}. This includes tax of {2}", bestQuote.Price, bestQuote.Name, bestQuote.Tax));
            //}

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
