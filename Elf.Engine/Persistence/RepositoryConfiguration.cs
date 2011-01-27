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

namespace Elf.Persistence {
    public class RepositoryConfiguration : IRepositoryConfiguration {
        IPersistenceConfigurer persistenceConfiguration;
        IAutomappingConfiguration autoMappingConfiguration;
        IAssemblySelector assemblySelector;

        NHibernate.Cfg.Configuration configuration;
        NHibernate.ISessionFactory sessionFactory;

        public RepositoryConfiguration(IPersistenceConfigurer persistenceConfiguration, IAutomappingConfiguration autoMappingConfiguration, IAssemblySelector assemblySelector) {
            this.persistenceConfiguration = persistenceConfiguration;
            this.autoMappingConfiguration = autoMappingConfiguration;
            this.assemblySelector = assemblySelector;

            configuration = Fluently.Configure()
                .Database(persistenceConfiguration)
                .Mappings(ConfigureMappings)
                .BuildConfiguration();
        }

        public NHibernate.Cfg.Configuration NHConfiguration {
            get { return configuration; }
        }

        private void ConfigureMappings(MappingConfiguration mapping) {
            mapping.AutoMappings.Add(CreateAutomappings);
        }

        private AutoPersistenceModel CreateAutomappings() {
            AutoPersistenceModel model = AutoMap.Assemblies(autoMappingConfiguration, assemblySelector.SelectAssemblies());
            model.Conventions.Add<CascadeConvention>();
            return model;
        }

    }
}
