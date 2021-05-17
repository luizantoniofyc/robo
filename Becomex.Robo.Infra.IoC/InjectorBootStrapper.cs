using Becomex.Robo.Application.Interfaces;
using Becomex.Robo.Application.UseCases;
using Bexomex.Robo.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Becomex.Robo.Infra.IoC
{
    public static class InjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Application
            services.AddScoped<IRobotUseCase, RobotUseCase>();
        }
    }
}
