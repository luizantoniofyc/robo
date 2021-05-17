using Microsoft.Extensions.DependencyInjection;
using System;

namespace Becomex.Robo.Infra.IoC
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDIConfiguration(this IServiceCollection services)
        {
            InjectorBootStrapper.RegisterServices(services);
        }
    }
}
