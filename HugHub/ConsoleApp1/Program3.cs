using ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program3
    {
        static readonly HttpClient s_client = new HttpClient
        {
            MaxResponseContentBufferSize = 1_000_000
        };

        static readonly IEnumerable<string> s_urlList = new string[]
        {
                "https://docs.microsoft.com",
                "https://docs.microsoft.com/aspnet/coe",
                "https://docs.microsoft.com/azure",
                "https://docs.microsoft.com/azure/devops",
                "https://docs.microsoft.com/dotnet",
                "https://docs.microsoft.com/dynamics365",
                "https://docs.microsoft.com/education",
                "https://docs.microsoft.com/enterprise-mobility-security",
                "https://docs.microsoft.com/gaming",
                "https://docs.microsoft.com/graph",
                "https://docs.microsoft.com/microsoft-365",
                "https://docs.microsoft.com/office",
                "https://docs.microsoft.com/powershell",
                "https://docs.microsoft.com/sql",
                "https://docs.microsoft.com/surface",
                "https://docs.microsoft.com/system-center",
                "https://docs.microsoft.com/visualstudio",
                "https://docs.microsoft.com/windows",
                "https://docs.microsoft.com/xamarin"
        };

        static Task Main() => SumPageSizesAsync();

        static async Task SumPageSizesAsync()
        {
            var stopwatch = Stopwatch.StartNew();

            IEnumerable<Task<InsurerQuoteModel>> downloadTasksQuery =
                from url in s_urlList
                select ProcessUrlAsync(url, s_client);

            List<Task<InsurerQuoteModel>> downloadTasks = downloadTasksQuery.ToList();

            InsurerQuoteModel bestQuote = null;
            while (downloadTasks.Any())
            {
                try
                {
                    //var response = await HttpClient.GetAsync(GetRandomNumberUrl);
                    Task<InsurerQuoteModel> finishedTask = await Task.WhenAny(downloadTasks);

                    downloadTasks.Remove(finishedTask);

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
                catch (WebException ex) 
                {
                    // handle 404 exceptions
                    Console.WriteLine("Timed out");
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
                    if (ex.InnerException is WebException webException && webException.Status == WebExceptionStatus.NameResolutionFailure)
                    {
                        Console.WriteLine("Connection failed." + ex);
                    }
                    //Note: The simplest way to see the error handling in action is to not run the service. 
                    //That will result in this error happening, instead of timeout.
                    Console.WriteLine("Connection failed." + ex.StatusCode + downloadTasks.ToString());
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
        }

        static async Task<InsurerQuoteModel> ProcessUrlAsync(string url, HttpClient client)
        {
            byte[] content = await client.GetByteArrayAsync(url);

            var returnModel = new InsurerQuoteModel()
            {
                Url = url,
                SiteContent = content,
                Price = content.Length,
                Tax = 0.2M,
            };
            returnModel.PriceTotal = returnModel.Price + (returnModel.Price * returnModel.Tax);

            Console.WriteLine(
                string.Format("Insurer: |{0,60}| Price: |{1,10}| Tax: |{2,10}| Total: |{3,10}|", returnModel.Url, returnModel.Price, returnModel.Tax, returnModel.PriceTotal));

            return returnModel;
        }
    }
}
