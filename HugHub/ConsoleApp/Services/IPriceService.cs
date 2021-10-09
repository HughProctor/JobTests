using System.Threading.Tasks;

namespace ConsoleApp._BusinessLogic
{
    public interface IPriceService
    {
        public Task<decimal> GetBestQuote();
    }
}
