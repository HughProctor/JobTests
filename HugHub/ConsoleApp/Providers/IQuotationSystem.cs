using ConsoleApp.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp.Providers
{
    public interface IQuotationSystem : IDisposable
    {
        string Url { get; set; }
        string Port { get; set; }
        IQuotationSystem CreateQuotationSystemList(RiskDataModel riskDataModel, HttpClient client);
        Task<InsurerQuoteModel> GetPrice();
    }

}
