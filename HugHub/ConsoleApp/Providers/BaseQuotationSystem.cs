using ConsoleApp.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp.Providers
{
    public abstract class BaseQuotationSystem : IQuotationSystem
    {
        public string Url { get; set; }
        public string Port { get; set; }
        public HttpClient Client { get; set; }
        public abstract IQuotationSystem CreateQuotationSystemList(RiskDataModel riskDataModel, HttpClient client);
        public abstract Task<InsurerQuoteModel> GetPrice();
        public void Dispose()
        {
            Client.CancelPendingRequests();
            Client = null;
        }

    }
}
