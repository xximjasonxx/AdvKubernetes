
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockPriceChangeConsumer;
using StockPriceChangeConsumer.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(configuration =>
    {
        configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables();
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddTransient<IConsumerClient, KafkaConsumeClient>();
        services.AddTransient<IStockPriceLookupService, CosmosSqlStockPriceLookupService>();

        services.AddHostedService<ApplicationHostedService>();
    });

// run the host
host.Start();