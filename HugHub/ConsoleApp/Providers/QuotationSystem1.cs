using ConsoleApp.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp.Providers
{
    public class QuotationSystem1 : BaseQuotationSystem, IQuotationSystem
    {
        #region Fields
        ISomeExternalService externalService;
        #endregion
        public QuotationSystem1(ISomeExternalService _someExternalService = null)
        {
            Url = "https://docs.microsoft.com/powershell"; // "http://quote-system-1.com";
            Port = "1234";
            externalService = _someExternalService;
        }

        public override IQuotationSystem CreateQuotationSystemList(RiskDataModel riskDataModel, HttpClient client)
        {
            Client = client;
            if (riskDataModel.DOB != null)
                return this;
            return null;
        }

        public override async Task<InsurerQuoteModel> GetPrice()
        {
            //var response = _someExternalService.PostHttpRequest(requestData);
            byte[] content = await Client.GetByteArrayAsync(Url);

            // This is just temp to ensure parallel processing works as expected
            var returnModel = new InsurerQuoteModel()
            {
                Name = "Test Name",
                Url = Url,
                SiteContent = content,
                Price = 123.45M,
                Tax = 123.45M * 0.12M,
            };
            returnModel.PriceTotal = returnModel.Price + (returnModel.Price * returnModel.Tax);
            return returnModel;
        }
    }
}
