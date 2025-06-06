using MassTransit;
using notification_service.Events.Consumers;
using notification_service.Events.Notifications;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<BookingConfirmedConsumer>();
    config.AddConsumer<OfferUpdatedConsumer>();

    config.SetEndpointNameFormatter(new DefaultEndpointNameFormatter("notification-service", false));

    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ")!);
        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddSignalR();
builder.Services.AddSingleton<INotificationSender, NotificationSender>();
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.SetIsOriginAllowed(_ => true);
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors();

app.UseWebSockets();

app.MapHub<NotificationHub>("/notifications");

app.Run();
