using Prometheus;

var countArgPosition = 0;
var iterationCount = int.Parse(args[countArgPosition]);

var registry = new CollectorRegistry();
var pusher = new MetricPusher(
    "http://localhost:9091/metrics",
    "point_collector",
    "default",
    intervalMilliseconds: 1000,
    additionalLabels: new List<Tuple<string, string>> {
        new ( "source", "metrics-test" )
    },
    registry);
pusher.Start();

var iterationDelaySeconds = int.Parse(System.Environment.GetEnvironmentVariable("ITERATION_DELAY_SECONDS", EnvironmentVariableTarget.Process)
    ?? "5");

//var pointCounter = factory.CreateCounter("point_counter", "A counter", includeTimestamp: true);
var pointCounter = Metrics.WithCustomRegistry(registry).CreateCounter("point_count", "A counter");

var currentCount = 0;
while (currentCount < iterationCount)
{
    pointCounter.Inc();
    Console.WriteLine($"Iteration {currentCount}");
    currentCount++;
    System.Threading.Thread.Sleep(iterationDelaySeconds * 1000);
}

await pusher.StopAsync();