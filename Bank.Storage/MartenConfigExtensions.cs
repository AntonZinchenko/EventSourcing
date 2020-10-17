using System;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bank.Storage
{
    public static class MartenConfigExtensions
    {
        public const string DefaultConfigKey = "EventStore";

        public static void AddMarten(this IServiceCollection services, IConfiguration config, Action<StoreOptions> configureOptions = null)
        {
            services.Configure<MartenOptions>(config.GetSection(DefaultConfigKey));
            services.AddSingleton<IDocumentStore>(
                sp => DocumentStore.For(options => SetStoreOptions(options, sp.GetRequiredService<IOptions<MartenOptions>>().Value, configureOptions)));
            services.AddScoped(sp => sp.GetRequiredService<IDocumentStore>().OpenSession());
        }

        private static void SetStoreOptions(StoreOptions options, MartenOptions config, Action<StoreOptions> configureOptions = null)
        {
            options.Connection(config.ConnectionString);
            options.AutoCreateSchemaObjects = AutoCreate.All;
            options.Events.DatabaseSchemaName = config.WriteModelSchema;
            options.DatabaseSchemaName = config.ReadModelSchema;

            configureOptions?.Invoke(options);
        }
    }
}
