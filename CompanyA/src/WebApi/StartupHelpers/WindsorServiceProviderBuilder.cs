namespace WebApi.StartupHelpers
{
    using System;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.MsDependencyInjection;

    using Core.Infrastructure.Repositories;
    using Core.Services.ImageAnalysis;
    using Core.Services.ImageAnalysis.ReferenceColorMatchingStrategies;

    using Infrastructure.StaticTestData;

    using Microsoft.Extensions.DependencyInjection;

    using Services;

    public class WindsorServiceProviderBuilder
    {
        public IServiceProvider Build(IServiceCollection services)
        {
            var container = new WindsorContainer();

            RegisterComponents(container);

            return WindsorRegistrationHelper.CreateServiceProvider(container, services);
        }

        private void RegisterComponents(WindsorContainer container)
        {
            RegisterWebApiServices(container);
            RegisterCoreServices(container);
            RegisterInfrastructure(container);
        }

        private static void RegisterInfrastructure(WindsorContainer container)
        {
            container.Register(Component.For<IReferenceColorRepository>().ImplementedBy<ReferenceColorRepository>().LifeStyle.Transient);
        }

        private static void RegisterCoreServices(WindsorContainer container)
        {
            container.Register(Component.For<IMeanColorCalculator>().ImplementedBy<MeanColorCalculator>().LifeStyle.Transient);
            container.Register(Component.For<IReferenceColorMatcher>().ImplementedBy<ReferenceColorMatcher>().LifeStyle.Transient);
            container.Register(Component.For<IReferenceColorMatchingStrategy>().ImplementedBy<MeanEuclideanDistanceReferenceColorMatchingStrategy>().LifeStyle.Transient);
        }

        private static void RegisterWebApiServices(WindsorContainer container)
        {
            container.Register(Component.For<IReferenceColorMatchingService>().ImplementedBy<ReferenceColorMatchingService>().LifeStyle.Transient);
        }
    }
}