using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions.Helpers;

namespace Elf.Persistence.Configuration {
    /// <summary>
    /// Describes the services that provides the persistence model to the repository configuration.
    /// </summary>
    public interface IPersistenceModelProvider {
        AutoPersistenceModel GetPersistenceModel();
    }

    /// <summary>
    /// Used to provide the persistence model to the repository configuration.
    /// </summary>
    public class DefaultPersistenceModelProvider : IPersistenceModelProvider {
        readonly AutoMapping.DefaultAutoMappingProvider configuration;
        readonly AssemblySelectors.IAssemblySelector assemblySelector;
        public DefaultPersistenceModelProvider(AutoMapping.DefaultAutoMappingProvider configuration, AssemblySelectors.IAssemblySelector assemblySelector) {
            this.configuration = configuration;
            this.assemblySelector = assemblySelector;
        }
        public virtual AutoPersistenceModel GetPersistenceModel() {
            AutoPersistenceModel model = AutoMap.Assemblies(configuration, assemblySelector.SelectAssemblies());
            AddConventions(model);
            return model;
        }
        protected virtual void AddConventions(AutoPersistenceModel model) {
            //TODO: Think about whether Cascade All is a good idea.
            model.Conventions.Add(DefaultCascade.All());
            //TODO: See if DefaultLazy.Always is the default
            model.Conventions.Add(DefaultLazy.Always());
        }
    }
}
