using MediatR;
using System.Threading.Tasks;

namespace SeedWorks.Core.Events
{
    public class EventBus: IEventBus
    {
        private readonly IMediator mediator;

        public EventBus(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task Publish(params IEvent[] events)
        {
            foreach (var @event in events)
            {
                await mediator.Publish(@event);
            }
        }
    }
}
