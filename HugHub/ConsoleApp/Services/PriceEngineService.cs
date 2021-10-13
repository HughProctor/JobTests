using ConsoleApp.Models;
using ConsoleApp.Providers;
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

    public interface ISomeExternalService { }

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
                    //throw new Exception(ex);
                }
            }

            stopwatch.Stop();

            //Console.WriteLine($"\nInsurer: |{bestQuote.Url,50}| Best Quote:  {bestQuote.Price:#,#}, Tax: {bestQuote.Tax:#,#}, Price Total: {bestQuote.PriceTotal:#,#}");
            //Console.WriteLine($"Elapsed time:          {stopwatch.Elapsed}\n");

            bestQuote.DateTime_Start = DateTime.Now;

            return bestQuote;
        }
    }
}
