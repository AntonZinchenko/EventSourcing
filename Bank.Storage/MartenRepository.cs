using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Baseline.Dates;
using Marten;
using Marten.Events.Projections.Async;
using Microsoft.Extensions.Options;
using Npgsql;
using SeedWorks;
using SeedWorks.Core.Aggregates;
using SeedWorks.Core.Events;
using SeedWorks.Core.Storage;

namespace BankAccount.Storage
{
    public class MartenRepository<T>: IRepository<T> where T : class, IAggregate, new()
    {
        private readonly IDocumentSession _documentSession;
        private readonly string _readModelSchema;
        private readonly IEventBus _eventBus;

        public MartenRepository(
            IDocumentSession documentSession,
            IEventBus eventBus,
            IOptions<MartenOptions> options
        )
        {
            _documentSession = documentSession ?? throw new ArgumentNullException(nameof(documentSession));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _readModelSchema = options.Value.ReadModelSchema;
        }

        public async Task<T> Find(Guid id, int version = default, CancellationToken cancellationToken = default)
        {
            // загрузка агрегата из снимка модели чтения
            var entity = await _documentSession.LoadAsync<T>(id, token: cancellationToken);
            if (entity == null)
                throw new EntityNotFoundException(id, typeof(T).Name);

            // начитка агрегата из журнала событий
            return await _documentSession.Events.AggregateStreamAsync<T>(id, version, token: cancellationToken);
        }

        public Task Add(T aggregate, CancellationToken cancellationToken)
            => _documentSession.Events.StartStream<T>(aggregate.Id)
                .PipeTo(_ => Store(aggregate, cancellationToken));

        public Task Update(T aggregate, CancellationToken cancellationToken)
            => Store(aggregate, cancellationToken);

        public Task Delete(T aggregate, CancellationToken cancellationToken) 
            => Store(aggregate, cancellationToken);

        private async Task Store(T aggregate, CancellationToken cancellationToken)
        {
            var events = aggregate.DequeueUncommittedEvents();
            _documentSession.Events.Append(
                aggregate.Id,
                events.ToArray()
            );

            await _documentSession.SaveChangesAsync(cancellationToken);

            await _eventBus.Publish(events.ToArray());
        }

        public async Task RebuildViews(Type[] views, CancellationToken cancellationToken)
        {
            if (!views?.Any() ?? false)
                return;

            foreach (var view in views)
            {
                using var cmd = new NpgsqlCommand($"DELETE FROM {_readModelSchema}.mt_doc_{view.Name}", _documentSession.Connection);
                await cmd.ExecuteNonQueryAsync(cancellationToken);
            }

            using var daemon = _documentSession.DocumentStore.BuildProjectionDaemon(views,
                settings: new DaemonSettings
                {
                    LeadingEdgeBuffer = 0.Seconds()
                });
            await daemon.RebuildAll(cancellationToken);
        }
    }
}
