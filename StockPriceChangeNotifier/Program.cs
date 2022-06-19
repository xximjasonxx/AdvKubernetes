﻿using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockPriceChangeNotifier;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(configuration =>
    {
        configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
            .AddEnvironmentVariables();
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddTransient<IConsumerClient, KafkaConsumeClient>();
        services.AddHostedService<ApplicationHostedService>();
    });

// run the host
host.Start();