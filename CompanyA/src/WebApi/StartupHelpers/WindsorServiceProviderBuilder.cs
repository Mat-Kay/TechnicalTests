namespace TechnicalTests.CompanyA.WebApi.StartupHelpers
{
    using System;

    using Castle.Windsor;
    using Castle.Windsor.MsDependencyInjection;

    using Microsoft.Extensions.DependencyInjection;

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
        }
    }
}