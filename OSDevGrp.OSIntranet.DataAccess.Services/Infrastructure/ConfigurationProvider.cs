﻿using Castle.MicroKernel.Registration;
using Castle.Windsor;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.IoC.Interfaces.Windsor;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Infrastructure
{
    /// <summary>
    /// Konfigurationsprovider til DataAccess.
    /// </summary>
    public class ConfigurationProvider : IConfigurationProvider
    {
        #region IConfigurationProvider Members

        /// <summary>
        /// Tilføjelse af konfiguration til containeren.
        /// </summary>
        /// <param name="container">Container, hvortil der skal tilføjes konfiguration.</param>
        public void AddConfiguration(IWindsorContainer container)
        {
            container.Register(Component.For<IDbAxConfiguration>().ImplementedBy<DbAxConfiguration>().LifeStyle.Transient);
            container.Register(Component.For<ILogRepository>().ImplementedBy<LogRepository>().LifeStyle.Transient);
            container.Register(Component.For<IObjectMapper>().ImplementedBy<ObjectMapper>().LifeStyle.Transient);
            container.Register(Component.For<IQueryBus>().ImplementedBy<QueryBus>().LifeStyle.Transient);
            container.Register(Component.For<ICommandBus>().ImplementedBy<CommandBus>().LifeStyle.Transient);

            container.Register(Classes.FromAssemblyNamed("OSDevGrp.OSIntranet.DataAccess.Services")
                .BasedOn<IRepository>()
                .WithService
                .AllInterfaces());

            container.Register(Classes.FromAssemblyNamed("OSDevGrp.OSIntranet.DataAccess.Services")
                .BasedOn<IQueryHandler>()
                .WithService
                .FromInterface(typeof(IQueryHandler)));

            container.Register(Classes.FromAssemblyNamed("OSDevGrp.OSIntranet.DataAccess.Services")
                .BasedOn<ICommandHandler>()
                .WithService
                .FromInterface(typeof(ICommandHandler)));

            container.Register(Classes.FromAssemblyNamed("OSDevGrp.OSIntranet.DataAccess.Services")
                .BasedOn<IRepositoryService>()
                .WithService
                .FromInterface(typeof(IRepositoryService)));
        }

        #endregion
    }
}
