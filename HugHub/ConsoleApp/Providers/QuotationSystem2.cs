using ConsoleApp.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp.Providers
{
    public class QuotationSystem2 : BaseQuotationSystem, IQuotationSystem
    {
        #region Fields
        ISomeExternalService externalService;
        #endregion
        public QuotationSystem2(ISomeExternalService _someExternalService = null)
        {
            Url = "https://docs.microsoft.com/dotnet";// "http://quote-system-12com";
            Port = "1235";
            externalService = _someExternalService;
        }

        public override IQuotationSystem CreateQuotationSystemList(RiskDataModel riskDataModel, HttpClient client)
        {
            Client = client;
            var makeList = new List<string> { "examplemake1", "examplemake2", "examplemake3" };

            if (!string.IsNullOrEmpty(riskDataModel.Make) && makeList.Contains(riskDataModel.Make.ToLower()))
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
                Name = "qewtrywrh",
                Url = Url,
                SiteContent = content,
                Price = 234.56M,
                Tax = 234.56M * 0.12M,
            };
            returnModel.PriceTotal = returnModel.Price + (returnModel.Price * returnModel.Tax);
            return returnModel;
        }
    }
}
