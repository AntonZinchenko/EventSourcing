using MassTransit;
using MediatR;
using System.Threading.Tasks;

namespace SeedWorks.Core.Events
{
    public class EventBus: IEventBus
    {
        private readonly IMediator _mediator;
        private readonly IBusControl _bus;

        public EventBus(IMediator mediator, IBusControl bus)
        {
            _mediator = mediator;
            _bus = bus;
        }

        public async Task Publish(params dynamic[] events)
        {
            foreach (var @event in events)
            {
                if (@event is INotification)
                    await _mediator.Publish(@event);

                await _bus.Publish(@event);
            }
        }
    }
}
