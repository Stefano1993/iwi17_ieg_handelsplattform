﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Net.Http;
using Newtonsoft.Json;
using CreationService.Model;
using System;

namespace CreationService
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
                c.SwaggerDoc("v1", new Info { Title = "Creation Service", Version = "v1" });
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Creation Service");
            });
            app.UseMvc();
            GetConfiguration();
        }

        public async void GetConfiguration()
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("http://iwi17-configurationservice.azurewebsites.net/api/Configuration/CreationService");
                Configuration responseContent = JsonConvert.DeserializeObject<Configuration>(response.Content.ReadAsStringAsync().Result);
                foreach (var config in responseContent.Configurations)
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
