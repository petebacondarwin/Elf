using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elf.Persistence;
using Elf.Persistence.Configuration;
using NHibernate;
using Elf.Persistence.Configuration.AssemblySelectors;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg.Db;

namespace Elf.Persistence.Modules {
    public abstract class CoreModule : Ninject.Modules.NinjectModule {
        public override void Load() {
            Kernel.Bind<IAssemblySelector>().To<AppDomainAssemblySelector>().InSingletonScope();
            Kernel.Bind<IAutomappingConfiguration>().To<AutoMappingConfiguration>().InSingletonScope();
            // We don't know what the database is yet so we are delegating to a method that can be overridden.
            Kernel.Bind<IPersistenceConfigurer>().ToMethod(GetDatabaseConfiguration);
            Kernel.Bind<NHibernate.Cfg.Configuration>().ToProvider<ConfigurationProvider>().InSingletonScope();
            Kernel.Bind<AutoPersistenceModel>().ToProvider<PersistenceModelProvider>().InSingletonScope();
            Kernel.Bind<ISessionFactory>().ToProvider<SessionFactoryProvider>().InSingletonScope();
            // We want the session to be bound to the HttpContext - if there isn't one then this is equivalent to TransientScope
            Kernel.Bind<ISession>().ToProvider<SessionProvider>().InRequestScope();
        }

        /// <summary>
        /// Implement this method to configure the database for NHibernate
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract IPersistenceConfigurer GetDatabaseConfiguration(Ninject.Activation.IContext context);
    }
}
