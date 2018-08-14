using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.ResolveAnything;
using Cohousing.Server.Api.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Cohousing.Server.Api.Startup
{
    public class AutofacConfig
    {
        public static IContainer Container { get; internal set; }
        
        public static IContainer BuildContainer(IEnumerable<ServiceDescriptor> serviceDescriptors) 
        {
            if (Container != null)
                return Container;
            
            var builder = new ContainerBuilder();

            AutoRegisterTypes(builder);
            RegisterTypes(builder);
            AddConventions(builder);

            // Pupulate services descripters
            builder.Populate(serviceDescriptors);

            Container = builder.Build();
            return Container;
        }

        private static void RegisterTypes(ContainerBuilder builder)
        {
            // Register explicit
            builder.RegisterType<TimeProvider>().As<ITimeProvider>().SingleInstance();
            builder.RegisterType<TimeFormatter>().As<ITimeFormatter>().SingleInstance();
        }

        private static void AutoRegisterTypes(ContainerBuilder builder)
        {
            // Auto Register interfaces and implementations
            var assemblies = new[]
            {
                Assembly.Load("Cohousing.Server.Service"),
                Assembly.Load("Cohousing.Server.Model"),
                Assembly.Load("Cohousing.Server.SqlRepository"),
            };
            builder.RegisterAssemblyTypes(assemblies).AsImplementedInterfaces();
        }

        private static void AddConventions(ContainerBuilder builder)
        {
            // Add conventions
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
        }
    }
}