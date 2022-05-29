using System.Reflection;
using StockDataWorker;
using StockDataWorker.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(configuration =>
    {
        configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: false)
            .AddEnvironmentVariables();
    })
    .ConfigureServices(services =>
    {
        services.AddTransient<SendStockDataService>();
        services.AddTransient<IStockDataQueryService, PolygonStockDataQueryService>();
        services.AddTransient<StockDataGenerationService>();
        services.AddTransient<StockPriceGenerationService>();
        services.AddTransient<IDataSendService, KafkaDataSendService>();

        services.AddSingleton<IProducerClient, KafkaProducerClient>();
        services.AddHostedService<TimerApplicationHostedService>();
    })
    .Build();

await host.RunAsync();
