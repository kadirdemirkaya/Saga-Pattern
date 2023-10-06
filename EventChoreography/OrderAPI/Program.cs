using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Consumers;
using OrderAPI.Data;
using SharedLIBRARY.Configurations;
using SharedLIBRARY.QueueEventNames;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

Assembly currentAssembly = Assembly.GetExecutingAssembly();
var mapperConfig = new MapperConfiguration(config =>
{
    config.AddMaps(currentAssembly);
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderDbContext>(opt =>
{
    opt.UseSqlServer(Configuration.GetDbSettings().ConnectionString);
});

builder.Services.AddMassTransit(mt =>
{
    mt.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(Configuration.GetRabbitMQSettings().RabbitMqHost, "/", host =>
        {
            host.Username(Configuration.GetRabbitMQSettings().RabbitMqUserName);
            host.Password(Configuration.GetRabbitMQSettings().RabbitMqPassword);
        });

        cfg.ReceiveEndpoint(EventQueues.BasketConfirmedEventQueueName, e =>
        {
            e.AutoDelete = true;
            e.ConfigureConsumer<BasketConfirmedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint(EventQueues.OrderStockNotEnoughtEventQueueName, e =>
        {
            e.AutoDelete = true;
            e.ConfigureConsumer<StockNotEnoughtEventConsumer>(context);
        });

        cfg.ReceiveEndpoint(EventQueues.OrderPaymentCompletedEventQueueName, e =>
        {
            e.AutoDelete = true;
            e.ConfigureConsumer<PaymentCompletedEventConsumer>(context);
        });
        cfg.ReceiveEndpoint(EventQueues.OrderPaymentNotCompletedEventQueueName, e =>
        {
            e.AutoDelete = true;
            e.ConfigureConsumer<PaymentNotCompletedEventConsumer>(context);
        });
    });

    mt.AddConsumer<BasketConfirmedEventConsumer>();
    mt.AddConsumer<StockNotEnoughtEventConsumer>();
    mt.AddConsumer<PaymentNotCompletedEventConsumer>();
    mt.AddConsumer<PaymentCompletedEventConsumer>();
});

builder.Services.AddMassTransitHostedService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
