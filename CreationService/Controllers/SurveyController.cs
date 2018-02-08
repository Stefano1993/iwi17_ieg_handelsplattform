using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CreationService.Model;
using System.Net.Http;
using Newtonsoft.Json;
using System;
using System.Text;

namespace CreationService.Controllers
{
    [Route("api/Survey")]
    public class SurveyController : Controller
    {
        // POST api/Survey
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Survey value)
        {
            Survey surveyToInsert = new Survey
            {
                FirstName = value.FirstName ?? ConfigurationManager.Configurations["FirstName"],
                LastName = value.LastName ?? ConfigurationManager.Configurations["LastName"],
                PersonAge = value.PersonAge ?? int.Parse(ConfigurationManager.Configurations["PersonAge"]),
                Prio1PaymentMethod = value.Prio1PaymentMethod ?? ConfigurationManager.Configurations["Prio1PaymentMethod"],
                Prio2PaymentMethod = value.Prio2PaymentMethod ?? ConfigurationManager.Configurations["Prio2PaymentMethod"],
                Prio3PaymentMethod = value.Prio3PaymentMethod ?? ConfigurationManager.Configurations["Prio3PaymentMethod"],
            };

            string storageServiceBaseUrl = await GetStorageServiceUrl();
            if (string.IsNullOrEmpty(storageServiceBaseUrl))
            {
                return BadRequest();
            }
            HttpClient httpClient = new HttpClient();
            string jsonSurvey = JsonConvert.SerializeObject(surveyToInsert);

            try
            {
                HttpContent content = new StringContent(jsonSurvey, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync($"{storageServiceBaseUrl}/api/Survey", content);
            }
            catch (Exception ex)
            {
                // TODO Add Logging
                return BadRequest();
            }

            return Ok();
        }

        private async Task<string> GetStorageServiceUrl()
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("http://iwi17-configurationservice.azurewebsites.net/api/Discovery/StorageService");
                Service responseContent = JsonConvert.DeserializeObject<Service>(response.Content.ReadAsStringAsync().Result);
                return responseContent.BaseUrl;
            }
            catch (Exception ex)
            {
                // TODO Add Logging
                return null;
            }
        }
    }
}
