using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Elf.Persistence.Configuration {
    /// <summary>
    /// Provides an NHibernate Session Factory to the injection container.
    /// </summary>
    public class SessionFactoryProvider : Ninject.Activation.Provider<ISessionFactory> {
        readonly NHibernate.Cfg.Configuration configuration;

        /// <summary>
        /// Create a new provider from an NHibernate configuration object
        /// </summary>
        /// <param name="configuration"></param>
        public SessionFactoryProvider(NHibernate.Cfg.Configuration configuration) {
            this.configuration = configuration;
        }

        /// <summary>
        /// Create the Session Factory from the configuration
        /// </summary>
        protected override ISessionFactory CreateInstance(Ninject.Activation.IContext context) {
            return configuration.BuildSessionFactory();
        }
    }
}