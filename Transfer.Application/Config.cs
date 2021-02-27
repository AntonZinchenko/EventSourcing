using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using SeedWorks;
using Transfer.Application.Mappers;
using MediatR;
using SeedWorks.Processing;

namespace Transfer.Application
{
    public static class Config
    {
        public static IServiceCollection ConfigApplication(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
                .ConfigMediatR(typeof(CommandHandler))
                .InitAutoMapper();

            return services;
        }

        public static IServiceCollection InitAutoMapper(this IServiceCollection services)
            => new MapperConfiguration(x => x.AddProfile<TransferStateToCommandProfile>())
                .PipeTo(config => services.AddSingleton(config.CreateMapper()));
    }
}
