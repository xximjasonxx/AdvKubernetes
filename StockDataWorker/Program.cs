using System.Reflection;
using StockDataWorker;
using StockDataWorker.Services;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: false)
    .AddEnvironmentVariables()
    .Build();

var serviceCollection = new ServiceCollection()
    .AddTransient<SendStockDataService>()
    .AddTransient<IStockDataQueryService, PolygonStockDataQueryService>()
    .AddTransient<StockDataGenerationService>()
    .AddTransient<StockPriceGenerationService>()
    .AddTransient<IDataSendService, KafkaDataSendService>()
    .AddSingleton<IProducerClient, KafkaProducerClient>()
    .AddLogging(configure => configure.AddConsole())
    .AddSingleton<IConfiguration>(configuration)
    .AddTransient<Application>();

using (var provider = serviceCollection.BuildServiceProvider())
{
    var application = provider.GetRequiredService<Application>();
    await application.Execute();
}
