using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Digipolis.Common.DataStore.Config;
using Narato.Common.ActionFilters;
using Newtonsoft.Json.Serialization;
using Narato.Common.DependencyInjection;
using Digipolis.Common.DependencyInjection;
using Digipolis.Iod_abs.Dossier.Common.Configuration;
using Digipolis.Iod_abs.Dossier.Domain.Managers;
using Digipolis.Iod_abs.Dossier.Domain.Interfaces;
using Digipolis.Iod_abs.Dossier.DataProvider.DataProviders;
using Digipolis.Iod_abs.Dossier.DataProvider.Interfaces;
using Digipolis.Iod_abs.Dossier.Domain.Clients;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Digipolis.Iod_abs.Dossier.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("config.json")
                .AddJsonFile("config.json.local", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DataStoreConfiguration>(Configuration.GetSection("DataStoreConfiguration"));
            services.Configure<TaskConfiguration>(Configuration.GetSection("TaskConfiguration"));

            // Add framework services.
            services.AddMvc(
                //Add this filter globally so every request runs this filter to recored execution time
                config =>
                {
                    config.Filters.Add(new ExecutionTimingFilter());
                    config.Filters.Add(new ModelValidationFilter());
                })
                //Add formatter for JSON output to client and to format received objects         
                .AddJsonOptions(x =>
                {
                    x.SerializerSettings.ContractResolver =
                     new CamelCasePropertyNamesContractResolver();
                }
            );

            services.AddCors();

            services.AddTransient<IDossierManager, DossierManager>();
            services.AddTransient<IDossierDataProvider, DossierDataProvider>();
            services.AddTransient<IDossierTypeConfigDataProvider, DossierTypeConfigDataProvider>();

            services.AddTransient<ITasksApiClient, TasksApiClient>(c =>
            {
                var config = c.GetService<IOptions<TaskConfiguration>>().Value;

                var client = new HttpClient() { BaseAddress = new Uri(config.BaseUrl) };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return new TasksApiClient(client);
            });
            

            services.AddDataStore(Configuration);
            services.AddNaratoCommon();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseMvc();
        }
    }
}
