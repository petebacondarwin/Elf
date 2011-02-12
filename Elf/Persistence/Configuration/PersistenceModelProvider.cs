namespace Elf.Persistence.Configuration {
    using System.Linq;
    using FluentNHibernate.Automapping;
    using FluentNHibernate.Conventions.Helpers;
    using System.Collections.Generic;
    using Elf.Configuration;

    /// <summary>
    /// Provide an Auto Persistence Model to the injection container.
    /// </summary>
    public class PersistenceModelProvider : Ninject.Activation.Provider<AutoPersistenceModel> {
        readonly IAutomappingConfiguration configuration;
        readonly AssemblyList assemblies;

        /// <summary>
        /// Create a new provider from an Auto Mapping configuration and an Assembly Selector
        /// </summary>
        /// <param name="configuration">The Auto Mapping configuration</param>
        /// <param name="assemblySelector">The Assembly Selector</param>
        public PersistenceModelProvider(IAutomappingConfiguration configuration, AssemblyList assemblies) {
            this.configuration = configuration;
            this.assemblies = assemblies;
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
            // Ensure that no dynamic assemblies are selected as the automapping does not like them
            AutoPersistenceModel model = AutoMap.Assemblies(configuration, assemblies.Where(assembly => !assembly.IsDynamic).ToArray());
            AddConventions(model);
            return model;
        }
    }
}