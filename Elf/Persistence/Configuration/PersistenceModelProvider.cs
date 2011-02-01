using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions.Helpers;

namespace Elf.Persistence.Configuration {
    /// <summary>
    /// Provide an Auto Persistence Model to the injection container.
    /// </summary>
    public class PersistenceModelProvider : Ninject.Activation.Provider<AutoPersistenceModel> {
        readonly IAutomappingConfiguration configuration;
        readonly AssemblySelectors.IAssemblySelector assemblySelector;

        /// <summary>
        /// Create a new provider from an Auto Mapping configuration and an Assembly Selector
        /// </summary>
        /// <param name="configuration">The Auto Mapping configuration</param>
        /// <param name="assemblySelector">The Assembly Selector</param>
        public PersistenceModelProvider(IAutomappingConfiguration configuration, AssemblySelectors.IAssemblySelector assemblySelector) {
            this.configuration = configuration;
            this.assemblySelector = assemblySelector;
        }
 
        /// <summary>
        /// Add Conventions to the generated Auto Persistence Model
        /// </summary>
        /// <param name="model">The model to which conventions are to be added</param>
        /// <remarks>
        /// Override this if you wish to modify the conventions on the model
        /// </remarks>
        protected virtual void AddConventions(AutoPersistenceModel model) {
            //TODO: Think about whether Cascade All is a good idea.
            model.Conventions.Add(DefaultCascade.All());
            //TODO: See if DefaultLazy.Always is the default
            model.Conventions.Add(DefaultLazy.Always());
        }

        /// <summary>
        /// Create an instance of the Auto Persistence Model
        /// </summary>
        protected override AutoPersistenceModel CreateInstance(Ninject.Activation.IContext context) {
            var assemblies = assemblySelector.SelectAssemblies();
            if (assemblies.Any(a => a.IsDynamic)) {
                throw new System.InvalidOperationException("The assembly selector must not select dynamic assemblies as the automapper cannot cope with them");
            }
            AutoPersistenceModel model = AutoMap.Assemblies(configuration, assemblies);
            AddConventions(model);
            return model;
        }
    }
}