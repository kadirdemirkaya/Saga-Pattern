using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StateMachineWorkerService.Models;

namespace StateMachineWorkerService.Maps
{
    public class OrderStateMap : SagaClassMap<OrderStateInstance>
    {
        protected override void Configure(EntityTypeBuilder<OrderStateInstance> entity, ModelBuilder model)
        {
            //entity.Property(p => p.BasketId).HasMaxLength(256);
            base.Configure(entity, model);
        }
    }
}
