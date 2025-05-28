using Generator;
using Generator.Services;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

// MassTransit z RabbitMQ i konsumentami
builder.Services.AddMassTransit(cfg =>
{
    cfg.SetEndpointNameFormatter(new DefaultEndpointNameFormatter("generator", false));

    cfg.UsingRabbitMq((context, rabbitCfg) =>
    {
        rabbitCfg.Host(builder.Configuration.GetConnectionString("RabbitMQ")!);

        rabbitCfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
        rabbitCfg.UseInMemoryOutbox();

        rabbitCfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddHttpClient<IOfferService, OfferService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5000/api/");
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
