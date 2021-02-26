using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace SeedWorks
{
    public static class Config
    {
        public static IServiceCollection ConfigMediatR(this IServiceCollection services, Type handlerType)
        {
            services.AddFluentValidation(new[] { handlerType.GetTypeInfo().Assembly });
            services.AddMediatR(handlerType);

            return services;
        }
    }
}
