using ConsoleApp._BusinessLogic;
using ConsoleApp.Resources;
using ConsoleApp1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace ConsoleApp.Controllers
{
    public class PriceController : ControllerBase
    {
        IPriceService _priceService;
        public PriceController(IPriceService priceService)
        {
            _priceService = priceService ?? new PriceService();
        }

        [HttpGet]
        public async Task<IActionResult> GetPriceAsync(RiskDataModel model)
        {
            if (model == null)
                return BadRequest(TResources.Controller_NullRequestBody);
                //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, TResources.Controller_NullRequestBody));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));

            try
            {
                var result = await _priceService.GetBestQuote();
                return Ok(result);
            }
            catch (Exception ex1)
            {
                return null; // InternalServerError(ex1);
                // log a error in trace writer.. handle in Elmah or Logger - this.traceWriter.Error
                //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex1));
            }
        }
    }
}
