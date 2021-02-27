using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Transfer.Api
{
    public class BusHostedService : IHostedService
    {
        private readonly IBusControl _busControl;

        public BusHostedService(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public Task StartAsync(CancellationToken cancellationToken)
            => _busControl.StartAsync(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken)
            => _busControl.StopAsync(cancellationToken);
    }
}
