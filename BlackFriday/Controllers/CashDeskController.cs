using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlackFriday.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using BlackFriday.Interfaces;
using BlackFriday.Dal;

namespace BlackFriday.Controllers
{
    [Produces("application/json")]
    [Route("api/CashDesk")]
    public class CashDeskController : Controller
    {

        private readonly ILogger<CashDeskController> _logger;
        private static readonly string creditcardServiceBaseAddress="http://iegeasycreditcardservice.azurewebsites.net/";
        private ISoldStatisticDal _statisticsDal;

        public CashDeskController(ILogger<CashDeskController> logger)
        {
            _logger = logger;
            _statisticsDal = new SoldStatisticDal();
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {

            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("{id:int}")]
        public string Get(int id)
        {

            return "value" + id;
        }


        [HttpPost]
        public IActionResult Post([FromBody]Basket basket)
        {
           _logger.LogError("TransactionInfo Creditcard: {0} Product:{1} Amount: {2}", new object[] { basket.CustomerCreditCardnumber, basket.Product, basket.AmountInEuro});

            //Mapping
            CreditcardTransaction creditCardTransaction = new CreditcardTransaction()
            {
                Amount = basket.AmountInEuro,
                CreditcardNumber = basket.CustomerCreditCardnumber,
                ReceiverName = basket.Vendor
            };

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(creditcardServiceBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response =  client.PostAsJsonAsync(creditcardServiceBaseAddress + "/api/CreditcardTransactions", creditCardTransaction).Result;
            response.EnsureSuccessStatusCode();

            ISell sell = new Sell
            {
                Amount = basket.AmountInEuro,
                Date = DateTimeOffset.Now,
                Product = basket.Product,
                Vendor = basket.Vendor
            };
            int id = _statisticsDal.AddSell(sell);

            return CreatedAtAction("Get", new { id = System.Guid.NewGuid() });
        }
    }
}