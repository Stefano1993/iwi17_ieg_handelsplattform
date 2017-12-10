using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Controllers.Models;
using Microsoft.Extensions.Logging;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller

    {
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Payment> Get()
        {
            List < Payment > test = new List<Payment>();
            Payment payment = new Payment();
            payment.Preis = 200;
            payment.Zahlungsart = "Kreditkarte";
            payment.Kontonummer = 12251;
            test.Add(payment);
            return test;
        }

        [HttpPost]
        public IActionResult Post([FromBody]Payment payment)
        {
            _logger.LogError("PaymentInfo Art: {0} Preis:{1} Kontonummer: {2}", new object[] { payment.Zahlungsart, payment.Preis, payment.Kontonummer });
            return CreatedAtAction("Get", new { id = System.Guid.NewGuid() });
        }

    }
}