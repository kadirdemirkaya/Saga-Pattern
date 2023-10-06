using AutoMapper;
using BasketAPI.Consumers;
using BasketAPI.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddDbContext<BasketDbContext>(opt =>
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

        cfg.ReceiveEndpoint(EventQueues.BasketCancelEventQueueName, e =>
        {
            e.AutoDelete = true;
            e.ConfigureConsumer<BasketCancelEventConsumer>(context);
        });

        cfg.ReceiveEndpoint(EventQueues.BasketStockNotEnoughtEventQueueName, e =>
        {
            e.AutoDelete = true;
            e.ConfigureConsumer<StockNotEnoughtEventConsumer>(context);
        });

        cfg.ReceiveEndpoint(EventQueues.BasketPaymentCompletedEventQueueName, e =>
        {
            e.AutoDelete = true;
            e.ConfigureConsumer<PaymentCompletedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint(EventQueues.BasketPaymentNotCompletedEventQueueName, e =>
        {
            e.AutoDelete = true;
            e.ConfigureConsumer<PaymentNotCompletedEventConsumer>(context);
        });
    });

    mt.AddConsumer<BasketCancelEventConsumer>();
    mt.AddConsumer<StockNotEnoughtEventConsumer>();
    mt.AddConsumer<PaymentCompletedEventConsumer>();
    mt.AddConsumer<PaymentNotCompletedEventConsumer>();

});

builder.Services.AddMassTransitHostedService();

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
