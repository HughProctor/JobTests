using ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class PriceRequest
    {
        public RiskDataModel RiskData;
    }

    public interface IQuotationSystem : IDisposable
    {
        string Url { get; set; }
        string Port { get; set; }
        IQuotationSystem CreateQuotationSystemList(RiskDataModel riskDataModel, HttpClient client);
        Task<InsurerQuoteModel> GetPrice();
    }

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

    public interface ISomeExternalService { }

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
                Price = content.Length,
                Tax = 0.2M,
            };
            returnModel.PriceTotal = returnModel.Price + (returnModel.Price * returnModel.Tax);
            return returnModel;

            //makes a call to an external service - SNIP
            //var response = _someExternalService.PostHttpRequest(requestData);

            //dynamic response = new ExpandoObject();
            //response.Price = 123.45M;
            //    response.IsSuccess = true;
            //    response.Name = "Test Name";
            //    response.Tax = 123.45M * 0.12M;

            //return response;
        }
    }

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
                Price = content.Length,
                Tax = 0.2M,
            };
            returnModel.PriceTotal = returnModel.Price + (returnModel.Price * returnModel.Tax);
            return returnModel;

            //makes a call to an external service - SNIP
            //var response = _someExternalService.PostHttpRequest(requestData);

            dynamic response = new ExpandoObject();
            response.Price = 234.56M;
            response.HasPrice = true;
            response.Name = "qewtrywrh";
            response.Tax = 234.56M * 0.12M;

            //return response;
        }
        //public dynamic GetPrice()
        //{
        //    //makes a call to an external service - SNIP
        //    //var response = _someExternalService.PostHttpRequest(requestData);

        //    dynamic response = new ExpandoObject();
        //    response.Price = 234.56M;
        //    response.HasPrice = true;
        //    response.Name = "qewtrywrh";
        //    response.Tax = 234.56M * 0.12M;

        //    return response;
        //}
    }

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
                Price = content.Length,
                Tax = 0.2M,
            };
            returnModel.PriceTotal = returnModel.Price + (returnModel.Price * returnModel.Tax);
            return returnModel;

            //makes a call to an external service - SNIP
            //var response = _someExternalService.PostHttpRequest(requestData);

            //dynamic response = new ExpandoObject();
            //response.Price = 92.67M;
            //response.IsSuccess = true;
            //response.Name = "zxcvbnm";
            //response.Tax = 92.67M * 0.12M;

            //return response;
        }
    }

    public class PriceEngineService
    {
        List<IQuotationSystem> s_qoutationSystemList;
        static readonly HttpClient s_client = new HttpClient
        {
            MaxResponseContentBufferSize = 1_000_000
        };

        public PriceEngineService(IQuotationSystem quotationSystem1 = null, IQuotationSystem quotationSystem2 = null, IQuotationSystem quotationSystem3 = null)
        {
            s_qoutationSystemList = new List<IQuotationSystem>()
            {
                quotationSystem1 ?? new QuotationSystem1(), 
                quotationSystem2 ?? new QuotationSystem2(), 
                quotationSystem3 ?? new QuotationSystem3()
            };
        }

        //pass request with risk data with details of a gadget, return the best price retrieved from 3 external quotation engines
        public async Task<InsurerQuoteModel> GetBestPrice(RiskDataModel riskDataModel)
        {
            var stopwatch = Stopwatch.StartNew();

            //initialise return variables
            var returnValue = new InsurerQuoteModel();

            //validation
            if (riskDataModel == null)
                throw new NullReferenceException("Risk Data Model cannot be null or empty");

            var context = new ValidationContext(riskDataModel, null, null);
            var results = new List<ValidationResult>();
            var isModelStateValid = Validator.TryValidateObject(riskDataModel, context, results, true);

            // Attach validation Results
            if (!isModelStateValid)
            {
                returnValue.ValidationResults = results;
                throw new ValidationException("Risk Data Model has incorrect data");
            }

            List<IQuotationSystem> qoutationSystemList = new List<IQuotationSystem>();

            s_qoutationSystemList.ForEach(x => {
                var quotationSystem = x.CreateQuotationSystemList(riskDataModel, s_client);
                if (quotationSystem != null) qoutationSystemList.Add(quotationSystem);
            });
            List<Task<InsurerQuoteModel>> insurancQuoteTaskList = (from item in qoutationSystemList
                                                                   select item.GetPrice()).ToList();
            InsurerQuoteModel bestQuote = null;
            while (insurancQuoteTaskList.Any())
            {
                try
                {
                    //var response = await HttpClient.GetAsync(GetRandomNumberUrl);
                    Task<InsurerQuoteModel> finishedTask = await Task.WhenAny(insurancQuoteTaskList);

                    insurancQuoteTaskList.Remove(finishedTask);

                    var tempModel = await finishedTask;

                    if (bestQuote == null) bestQuote = tempModel;
                    else
                    {
                        if (bestQuote.PriceTotal < tempModel.PriceTotal)
                            bestQuote = tempModel;
                    }
                    //if (response.StatusCode != HttpStatusCode.OK)
                    //{
                    //    TripCircuit(reason: $"Status not OK. Status={response.StatusCode}");
                    //    return UNAVAILABLE;
                    //}

                    //return await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex) when (ex is OperationCanceledException || ex is TaskCanceledException)
                {
                    //Note: If you want to simulate timeouts to see this error handling path, I suggest using toxiproxy.
                    //Ref: https://makolyte.com/how-to-use-toxiproxy-to-verify-your-code-can-handle-timeouts-and-unavailable-endpoints/
                    Console.WriteLine("Timed out");
                    //return UNAVAILABLE;
                }
                catch (HttpRequestException ex) //when (ex?.InnerException is SocketException sockEx && sockEx.SocketErrorCode == SocketError.ConnectionRefused)
                {
                    //Note: The simplest way to see the error handling in action is to not run the service. 
                    //That will result in this error happening, instead of timeout.
                    Console.WriteLine("Connection failed." + ex.StatusCode + insurancQuoteTaskList.ToString());
                    //var status = ((WebException)ex).Status;
                    //return UNAVAILABLE;
                }
                //finally
                //{
                //    semaphore.Release();
                //}

            }

            stopwatch.Stop();

            Console.WriteLine($"\nInsurer: |{bestQuote.Url,50}| Best Quote:  {bestQuote.Price:#,#}, Tax: {bestQuote.Tax:#,#}, Price Total: {bestQuote.PriceTotal:#,#}");
            Console.WriteLine($"Elapsed time:          {stopwatch.Elapsed}\n");

            bestQuote.DateTime_Start = DateTime.Now;

            return bestQuote;
            //now call 3 external system and get the best price
            #region Old Code
            //system 1 requires DOB to be specified
            //if (riskDataModel.DOB != null)
            //{
            //    QuotationSystem1 system1 = new QuotationSystem1("http://quote-system-1.com", "1234");

            //    dynamic systemRequest1 = new ExpandoObject();
            //    systemRequest1.FirstName = request.RiskData.FirstName;
            //    systemRequest1.Surname = request.RiskData.LastName;
            //    systemRequest1.DOB = request.RiskData.DOB;
            //    systemRequest1.Make = request.RiskData.Make;
            //    systemRequest1.Amount = request.RiskData.Value;

            //    dynamic system1Response = system1.GetPrice(systemRequest1);
            //    if (system1Response.IsSuccess)
            //    {
            //        returnValue.Price = system1Response.Price;
            //        returnValue.Name = system1Response.Name;
            //        returnValue.Tax = system1Response.Tax;
            //    }
            //}

            ////system 2 only quotes for some makes
            //if (!string.IsNullOrEmpty(riskDataModel.Make) && (riskDataModel.Make == "examplemake1" || riskDataModel.Make == "examplemake2" ||
            //    riskDataModel.Make == "examplemake3"))
            //{
            //    dynamic systemRequest2 = new ExpandoObject();
            //    systemRequest2.FirstName = request.RiskData.FirstName;
            //    systemRequest2.LastName = request.RiskData.LastName;
            //    systemRequest2.Make = request.RiskData.Make;
            //    systemRequest2.Value = request.RiskData.Value;

            //    QuotationSystem2 system2 = new QuotationSystem2("http://quote-system-2.com", "1235", systemRequest2);

            //    dynamic system2Response = system2.GetPrice();
            //    if (system2Response.HasPrice && system2Response.Price < price)
            //    {
            //        price = system2Response.Price;
            //        insurerName = system2Response.Name;
            //        tax = system2Response.Tax;
            //    }
            //}

            ////system 3 is always called
            //QuotationSystem3 system3 = new QuotationSystem3("http://quote-system-3.com", "100");
            //dynamic systemRequest3 = new ExpandoObject();

            //systemRequest3.FirstName = request.RiskData.FirstName;
            //systemRequest3.Surname = request.RiskData.LastName;
            //systemRequest3.DOB = request.RiskData.DOB;
            //systemRequest3.Make = request.RiskData.Make;
            //systemRequest3.Amount = request.RiskData.Value;

            //var system3Response = system3.GetPrice(systemRequest3);
            //if (system3Response.IsSuccess && system3Response.Price < price)
            //{
            //    price = system3Response.Price;
            //    insurerName = system3Response.Name;
            //    tax = system3Response.Tax;
            //}

            //if (price == 0)
            //{
            //    price = -1;
            //}

            //return price;
            #endregion Old Code
        }
    }
}
