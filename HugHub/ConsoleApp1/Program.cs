using ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
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


            decimal tax = 0;
            string insurer = "";
            string error = "";

            var priceEngine = new ConsoleApp1.PriceEngine();
            var insurerQuote = priceEngine.GetPrice(request.RiskData);

            if (insurerQuote == -1)
            {
                Console.WriteLine(String.Format("There was an error - {0}", error));
            }
            else
            {
                Console.WriteLine(String.Format("You price is {0}, from insurer: {1}. This includes tax of {2}", insurerQuote.Price, insurerQuote.Name, insurerQuote.Tax));
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
