namespace Naos.Sample.Customers.Application.Web
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    public static class Program
    {
        public static readonly string AppName = typeof(Program).Namespace.Replace("Naos.Sample.", string.Empty, StringComparison.OrdinalIgnoreCase).Replace(".Application.Web", string.Empty, StringComparison.OrdinalIgnoreCase);

        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();
            Log.Logger = CreateSerilogLogger(configuration);

            try
            {
                Log.Information("configuring web host (service={ServiceName})...", AppName);
                var host = BuildWebHost(configuration, args);

                Log.Information("starting web host (service={ServiceName})...", AppName);
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "program terminated unexpectedly (service={ServiceName})!", AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(false)
                .UseStartup<Startup>()
                //.UseApplicationInsights()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(configuration)
                .UseSerilog()
                .Build();

        private static ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ServiceName", AppName)
                .Enrich.FromLogContext()
                .WriteTo.Trace()
                .WriteTo.Console()
                .WriteTo.Seq(string.IsNullOrWhiteSpace(configuration["Serilog:SeqServerUrl"]) ? "http://seq" : configuration["Serilog:SeqServerUrl"])
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            if (config.GetValue<bool>("AzureAppConfiguration:Enabled", false))
            {
                builder.AddAzureAppConfiguration(config["AzureAppConfiguration:ConnectionString"]);
            }

            if (config.GetValue<bool>("AzureKeyVault:Enabled", false))
            {
                builder.AddAzureKeyVault(
                    $"https://{config["AzureKeyVault:Name"]}.vault.azure.net/",
                    config["AzureKeyVault:ClientId"],
                    config["AzureKeyVault:ClientSecret"]);
            }

            return builder.Build();
        }
    }
}
