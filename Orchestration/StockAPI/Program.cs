using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SharedLIBRARY.Configurations;
using SharedLIBRARY.QueueEventNames;
using StockAPI.Consumers;
using StockAPI.Data;
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

builder.Services.AddDbContext<StockDbContext>(opt =>
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

        cfg.ReceiveEndpoint(EventQueues.OrderCreatedEventQueueName, e =>
        {
            e.AutoDelete = true;
            e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint(EventQueues.StockPaymentCompletedEventQueueName, e =>
        {
            e.AutoDelete = true;
            e.ConfigureConsumer<PaymentCompletedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint(EventQueues.StockPaymentNotCompletedEventQueueName, e =>
        {
            e.AutoDelete = true;
            e.ConfigureConsumer<PaymentNotCompletedEventConsumer>(context);
        });
    });

    mt.AddConsumer<OrderCreatedEventConsumer>();
    mt.AddConsumer<PaymentNotCompletedEventConsumer>();
    mt.AddConsumer<PaymentCompletedEventConsumer>();
});

//builder.Services.AddMassTransitHostedService();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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