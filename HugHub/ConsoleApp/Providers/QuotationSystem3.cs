using ConsoleApp.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp.Providers
{
    public class QuotationSystem3 : BaseQuotationSystem, IQuotationSystem
    {
        #region Fields
        ISomeExternalService externalService;
        #endregion
        public QuotationSystem3(ISomeExternalService _someExternalService = null)
        {
            Url = "https://docs.microsoft.com/azure"; // "http://quote-system-3.com";
            Port = "100";
            externalService = _someExternalService;
        }

        public override IQuotationSystem CreateQuotationSystemList(RiskDataModel riskDataModel, HttpClient client)
        {
            Client = client;
            return this;
        }

        public override async Task<InsurerQuoteModel> GetPrice()
        {
            //var response = _someExternalService.PostHttpRequest(requestData);
            byte[] content = await Client.GetByteArrayAsync(Url);

            // This is just temp to ensure parallel processing works as expected
            var returnModel = new InsurerQuoteModel()
            {
                Name = "zxcvbnm",
                Url = Url,
                SiteContent = content,
                Price = 92.67M,
                Tax = 92.67M * 0.12M,
            };
            returnModel.PriceTotal = returnModel.Price + (returnModel.Price * returnModel.Tax);
            return returnModel;
        }
    }

}
