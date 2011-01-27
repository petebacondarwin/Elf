using System;
using FluentNHibernate.Automapping;

namespace Elf.Persistence {
    public class PersistenceModelProvider : IPersistenceModelProvider{
        AutomappingConfiguration configuration;
        IAssemblySelector assemblySelector;
        public PersistenceModelProvider(AutomappingConfiguration configuration, IAssemblySelector assemblySelector) {
            this.configuration = configuration;
            this.assemblySelector = assemblySelector;
        }
        public virtual AutoPersistenceModel GetPersistenceModel() {
            AutoPersistenceModel model = AutoMap.Assemblies(configuration, assemblySelector.SelectAssemblies());
            AddConventions(model);
            return model;
        }
        protected virtual void AddConventions(AutoPersistenceModel model) {
            model.Conventions.Add<CascadeConvention>();
        }
    }
}
