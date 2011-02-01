using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace Elf.Persistence.Configuration {
    /// <summary>
    /// Provides an NHibernate Configuration to the injection container.
    /// </summary>
    public class ConfigurationProvider : Ninject.Activation.Provider<NHibernate.Cfg.Configuration> {
        readonly IPersistenceConfigurer databaseConfiguration;
        readonly AutoPersistenceModel persistenceModel;

        /// <summary>
        /// Create a new provider from a database configuration and a persistence model
        /// </summary>
        public ConfigurationProvider(IPersistenceConfigurer databaseConfiguration, AutoPersistenceModel persistenceModel) {
            this.databaseConfiguration = databaseConfiguration;
            this.persistenceModel = persistenceModel;
        }

        /// <summary>
        /// Create an instance of the configuration from the database configuration and persistence model
        /// </summary>
        protected override NHibernate.Cfg.Configuration CreateInstance(Ninject.Activation.IContext context) {
            return Fluently.Configure()
                .Database(databaseConfiguration)
                .Mappings(mappings => mappings.AutoMappings.Add(persistenceModel))
                .BuildConfiguration();
        }
    }
}
