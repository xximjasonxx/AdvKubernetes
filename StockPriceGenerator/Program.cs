
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StockPriceGenerator;
using StockPriceGenerator.Services;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true)
    .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
    .AddEnvironmentVariables()
    .Build();

var serviceCollection = new ServiceCollection()
  .AddLogging(configure => configure.AddConsole())
  .AddSingleton<IStockPicksReadService, RedisStockPicksReadService>()
  .AddTransient<IProducerClient, KafkaProducerClient>()
  .AddTransient<PriceGenerationService>()
  .AddSingleton<IConfiguration>(configuration)
  .AddTransient<Application>();

using (var provider = serviceCollection.BuildServiceProvider())
{
    var application = provider.GetRequiredService<Application>();
    await application.Execute();
}