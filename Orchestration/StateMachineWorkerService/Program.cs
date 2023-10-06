using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLIBRARY.Configurations;
using SharedLIBRARY.QueueEventNames;
using StateMachineWorkerService;
using StateMachineWorkerService.Data;
using StateMachineWorkerService.Models;
using StateMachineWorkerService.StateMachines;
using System.Reflection;

Microsoft.Extensions.Hosting.IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(cfg =>
        {
            cfg.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>().EntityFrameworkRepository(opt =>
            {
                opt.AddDbContext<DbContext, OrderStateDbContext>((provider, builder) =>
                {
                    builder.UseSqlServer(Configuration.GetDbSettings().ConnectionString, m =>
                    {
                        m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                    });
                });
            });

            cfg.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(configure =>
            {
                configure.Host(Configuration.GetRabbitMQSettings().RabbitMqHost, "/", host =>
                {
                    host.Username(Configuration.GetRabbitMQSettings().RabbitMqUserName);
                    host.Password(Configuration.GetRabbitMQSettings().RabbitMqPassword);
                });

                configure.ReceiveEndpoint(EventQueues.OrderSagaQueueName, e =>
                {
                    e.ConfigureSaga<OrderStateInstance>(provider);
                });
            }));

            services.AddHostedService<Worker>();
        });
    }).Build();

host.Run();