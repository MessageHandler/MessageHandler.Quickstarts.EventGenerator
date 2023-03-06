using Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddLogging();

        

        services.AddHostedService<GenerateEvents>();
     
    })
    .Build();

await host.RunAsync();
