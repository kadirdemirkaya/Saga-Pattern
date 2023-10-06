using Microsoft.Extensions.Hosting;

namespace StateMachineWorkerService
{
    public class Worker : BackgroundService
    {
        //public override Task StartAsync(CancellationToken cancellationToken)
        //{
        //    return base.StartAsync(cancellationToken);
        //}

        //public override Task StopAsync(CancellationToken cancellationToken)
        //{
        //    return base.StopAsync(cancellationToken);
        //}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //throw new NotImplementedException();
        }

        //public override Task? ExecuteTask => base.ExecuteTask;
    }
}
