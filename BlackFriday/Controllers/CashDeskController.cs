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
        private static readonly string creditcardServiceBaseAddress = "http://iegeasycreditcardservice.azurewebsites.net/";
        private ISoldStatisticDal _statisticsDal;
        private static int _repeats = 3;
        private static int _usedService = 0;

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
            _logger.LogError("TransactionInfo Creditcard: {0} Product:{1} Amount: {2}", new object[] { basket.CustomerCreditCardnumber, basket.Product, basket.AmountInEuro });

            bool postSuccessful = false;
            int currentRepeats = 0;
            HttpResponseMessage response;

            List<string> creditCardServices = new List<string>();
            creditCardServices.Add("http://iegeasycreditcardservice.azurewebsites.net/");
            creditCardServices.Add("http://iwi17-easycreditcardservice.azurewebsites.net");
            creditCardServices.Add("http://iwi17-easycreditcardservice2.azurewebsites.net/");
            creditCardServices.Add("http://iwi17-easycreditcardservice3.azurewebsites.net/");

            int availableServices = creditCardServices.Count;
            int triedServices = 0;
            bool exhaustedRetries = false;

            //Mapping
            CreditcardTransaction creditCardTransaction = new CreditcardTransaction()
            {
                Amount = basket.AmountInEuro,
                CreditcardNumber = basket.CustomerCreditCardnumber,
                ReceiverName = basket.Vendor
            };

            do
            {
                HttpClient client = new HttpClient();

                //client.BaseAddress = new Uri(creditcardServiceBaseAddress);
                client.BaseAddress = new Uri(creditCardServices[_usedService]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    response = client.PostAsJsonAsync(creditCardServices[_usedService] + "/api/CreditcardTransactions", creditCardTransaction).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogError("TransactionInfo Creditcard: Successful. Retry: {0}, Server: {1} (#{2})", new object[] { currentRepeats + 1, creditCardServices[_usedService], _usedService + 1 });
                        postSuccessful = true;
                        // rotate round robin
                        _usedService = (_usedService + 1) % availableServices;

                    }
                    else throw (new HttpRequestException("No Success Status Code Received."));


                    //response.EnsureSuccessStatusCode();
                }
                catch (Exception errorHTTP)
                {

                    _logger.LogError("TransactionInfo Creditcard: failed to Connect to Service via HTTP. Retry: {0}, Error: {1}", new object[] { currentRepeats, errorHTTP.Message });

                    if (currentRepeats < _repeats)
                    {
                        currentRepeats = currentRepeats + 1;
                    }

                    else
                    {
                        if (triedServices < availableServices)
                        {
                            // choose another Service or quit
                            _logger.LogError("TransactionInfo Creditcard: failed to Connect to Service via HTTP. Retry count reached, trying another service.");
                            _usedService = (_usedService + 1) % availableServices;
                            triedServices = triedServices + 1;
                        }
                        else
                        {
                            exhaustedRetries = true;
                            _logger.LogError("TransactionInfo Creditcard: No more Services to try. Aborting Request.");
                            return StatusCode(503); // Internal Server Error - Service Unavailable.;
                        }
                    }

                }


            } while (!postSuccessful && !exhaustedRetries);

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