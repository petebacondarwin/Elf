using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate;

namespace Elf.Persistence {
    public class RepositoryConfiguration : IRepositoryConfiguration {
        #region Depends Upon
        IPersistenceConfigurer persistenceConfiguration;
        IPersistenceModelProvider persistenceModelProvider;
        #endregion Depends Upon

        #region Provides
        NHibernate.Cfg.Configuration configuration;
        public NHibernate.Cfg.Configuration NHConfiguration { get { return configuration; } }
        #endregion Provides


        public RepositoryConfiguration(IPersistenceConfigurer persistenceConfiguration, IPersistenceModelProvider persistenceModelProvider) {
            this.persistenceConfiguration = persistenceConfiguration;
            this.persistenceModelProvider = persistenceModelProvider;

            configuration = Fluently.Configure()
                .Database(persistenceConfiguration)
                .Mappings(ConfigureMappings)
                .BuildConfiguration();
        }

        private void ConfigureMappings(MappingConfiguration mapping) {
            mapping.AutoMappings.Add(persistenceModelProvider.GetPersistenceModel());
        }
    }
}
