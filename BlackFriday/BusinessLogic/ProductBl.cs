using BlackFriday.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlackFriday.BusinessLogic
{
    public class ProductBl
    {
        private IConfiguration _configuration;
        private static HttpClient _httpClient;

        public ProductBl(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<List<Product>> GetProducts()
        {
            try {
                string baseUrl = _configuration["ProductServiceLocalBaseUrl"];
                string url = $"{baseUrl}/api/Products";

                var products = new List<Product>();

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    products = await response.Content.ReadAsAsync<List<Product>>();
                }

                return products;
            }
            catch (Exception ex)
            {
                // TODO Add Logging
                throw;
            }
        }
    }
}
