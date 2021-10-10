using ConsoleApp.Models;
using System;
using System.Threading.Tasks;

namespace ConsoleApp._BusinessLogic
{
    internal class PriceService : IPriceService
    {
        public Task<InsurerQuoteModel> GetBestInsurereQuote()
        {
            throw new NotImplementedException();
        }

        Task<decimal> IPriceService.GetBestQuote()
        {
            throw new NotImplementedException();
        }

    }
}
