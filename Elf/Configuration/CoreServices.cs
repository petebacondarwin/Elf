namespace Elf.Configuration {
    using Elf.Persistence;
    using Elf.Persistence.Configuration;
    using FluentNHibernate.Automapping;
    using NHibernate;
    using Ninject.Modules;

    /// <summary>
    /// The core Ninject bindings.
    /// </summary>
    public class CoreServices : NinjectModule {
        /// <summary>
        /// Load the bindings specified in this module into the kernel.
        /// </summary>
        public override void Load() {
            // Bind the NHibernate configuration types
            Kernel.Bind<IAutomappingConfiguration>().To<AutoMappingConfiguration>().InSingletonScope();
            Kernel.Bind<NHibernate.Cfg.Configuration>().ToProvider<ConfigurationProvider>().InSingletonScope();
            Kernel.Bind<AutoPersistenceModel>().ToProvider<PersistenceModelProvider>().InSingletonScope();
            Kernel.Bind<ISessionFactory>().ToProvider<SessionFactoryProvider>().InSingletonScope();

            // We want the session to be bound to the HttpContext - if there isn't one then this is equivalent to TransientScope
            Kernel.Bind<ISession>().ToProvider<SessionProvider>().InRequestScope();
            Kernel.Bind<IContentFinder>().To<ContentFinder>().InRequestScope();
        }
    }
}
