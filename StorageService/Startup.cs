using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StorageService.Model;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace StorageService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Storage Service", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Storage Service");
            });
            app.UseMvc();

            RegisterService();
            GetConfiguration();
        }

        public async void RegisterService()
        {
            HttpClient httpClient = new HttpClient();
            // For the amount of storage services which should be deployed, register an instance for the discovery
            for (int i = 1; i < 3; i++)
            {
                ServiceSubscription subscription = new ServiceSubscription
                {
                    BaseUrl = $"http://iwi17-storageservice{i}.azurewebsites.net",
                    HealthCheckRoute = "/swagger",
                    ServiceName = "StorageService"
                };

                string jsonSubscription = JsonConvert.SerializeObject(subscription);

                try
                {
                    HttpContent content = new StringContent(jsonSubscription, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync("http://iwi17-configurationservice.azurewebsites.net/api/Discovery", content);
                }
                catch (Exception ex)
                {
                    // TODO Add Logging
                }
            }
        }

        public async void GetConfiguration()
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("http://iwi17-configurationservice.azurewebsites.net/api/Configuration/StorageService");
                Configuration responseContent = JsonConvert.DeserializeObject<Configuration>(response.Content.ReadAsStringAsync().Result);
                foreach(var config in responseContent.Configurations)
                {
                    ConfigurationManager.Configurations.Add(config.Key, config.Value);
                }
            }
            catch (Exception ex)
            {
                // TODO Add Logging
            }
        }
    }
}
