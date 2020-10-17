using System.Threading.Tasks;

namespace SeedWorks.Core.Events
{
    public interface IEventBus
    {
        Task Publish(params IEvent[] events);
    }
}
