using MassTransit;
using Microsoft.EntityFrameworkCore;
using OfferService.Events;
using OfferService.Events.Consumers;
using OfferService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OfferContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("MySql")));

builder.Services.AddMassTransit(config => {
    config.AddConsumer<BookingCancelledConsumer>();
    config.AddConsumer<BookingCreatedConsumer>();
    config.AddConsumer<BookingExpiredConsumer>();

    config.SetEndpointNameFormatter(new DefaultEndpointNameFormatter("offer-service", false));

    config.UsingRabbitMq((context, cfg) => {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ")!);
        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddTransient<Publisher>();
builder.Services.AddScoped<OfferService.Services.OfferService>();
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.SetIsOriginAllowed(_ => true);
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OfferContext>();
    context.Database.EnsureCreated();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
