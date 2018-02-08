using ConfigurationService.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConfigurationService.Helper
{
    public class HealthChecker
    {
        public List<Service> Services;
        private HttpClient _httpClient;

        private static HealthChecker instance;

        private HealthChecker()
        {
            _httpClient = new HttpClient();
            Services = new List<Service>();
        }

        public static HealthChecker Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HealthChecker();
                }
                return instance;
            }
        }

        public async Task Check(string serviceName = null)
        {
            foreach(var service in Services)
            {
                if (!string.IsNullOrEmpty(serviceName))
                {
                    if(service.ServiceName != serviceName)
                    {
                        continue;
                    }
                }
                try
                {
                    HttpResponseMessage response = await _httpClient.GetAsync($"{service.BaseUrl}{service.HealthCheckRoute}");
                    if (response.IsSuccessStatusCode)
                    {
                        service.Healthy = true;
                        continue;
                    }
                }
                catch(Exception ex)
                {
                    // TODO Add Logging
                }
                service.Healthy = false;
            }
        }
    }
}
