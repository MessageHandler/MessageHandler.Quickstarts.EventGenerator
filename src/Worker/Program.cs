using Worker;
using MessageHandler.Runtime;
using MessageHandler.Runtime.StreamProcessing;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddLogging();

        var eventhubsnamespace = hostContext.Configuration.GetValue<string>("eventhubsnamespace")
                                        ?? throw new Exception("No 'eventhubsnamespace' was provided. Use User Secrets or specify via environment variable.");

        services.AddMessageHandler("orderbooking.worker", runtimeConfiguration =>
        {
            runtimeConfiguration.BufferedDispatchingPipeline(options =>
            {
                options.SerializeMessagesWith(new JSonMessageSerializer());
                options.RouteMessages(to => to.EventHub("largehub", eventhubsnamespace));
            });
        });

        services.AddSingleton<IGenerateEvents, ReportDataDownloadedViaWifi>();
        services.AddHostedService<RunEventGenerator>();
     
    })
    .Build();

await host.RunAsync();