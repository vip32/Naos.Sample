namespace Naos.Sample.ApiGateway.Application.Web
{
    using System;
    using HealthChecks.UI.Client;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Ocelot.DependencyInjection;
    using Ocelot.Middleware;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("ocelot.json")
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddUrlGroup(new Uri("http://customers.application.web/health"), name: "customers.application.web", tags: new string[] { "customers.application.web" })
                .AddUrlGroup(new Uri("http://orders.application.web/health"), name: "orders.application.web", tags: new string[] { "orders.application.web" });
            // TODO: get hosts from ocelot file?

            services.AddOcelot(this.Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.MapWhen(c => c.Request.Path == "/", a =>
            {
                a.Run(async x =>
                {
                    await x.Response.WriteAsync($"<html><body>{this.GetType().Namespace}&nbsp;<a href='/health'>health</a>&nbsp;<a href='/liveness'>liveness</a></body></html>").ConfigureAwait(false);
                });
            });

            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self", StringComparison.OrdinalIgnoreCase)
            });

            app.UseHttpsRedirection();
            app.UseOcelot().Wait();
        }
    }
}
