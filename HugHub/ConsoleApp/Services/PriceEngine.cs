using System;
using System.Threading.Tasks;

namespace ConsoleApp._BusinessLogic
{
    internal class PriceService : IPriceService
    {
        Task<decimal> IPriceService.GetBestQuote()
        {
            throw new NotImplementedException();
        }
    }
}
