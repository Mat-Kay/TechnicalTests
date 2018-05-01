namespace WebApi.StartupHelpers
{
    using System;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.MsDependencyInjection;

    using Core.Infrastructure.Repositories;
    using Core.Services.ImageAnalysis;

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
            container.Register(Component.For<IReferenceColorMatchingService>().ImplementedBy<ReferenceColorMatchingService>().LifeStyle.Transient);

            container.Register(Component.For<IMeanColorCalculator>().ImplementedBy<MeanColorCalculator>().LifeStyle.Transient);
            container.Register(Component.For<IReferenceColorMatchingStrategy>().ImplementedBy<MeanEuclideanDistanceReferenceColorMatcher>().LifeStyle.Transient);
            container.Register(Component.For<IReferenceColorRepository>().ImplementedBy<ReferenceColorRepository>().LifeStyle.Transient);
        }
    }
}