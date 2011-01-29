using System;
using System.Reflection;

namespace Elf.Persistence.Configuration.AssemblySelectors {
    /// <summary>
    /// Provides access to the specified assemblies.
    /// </summary>
    public class SpecifiedAssemblySelector : IAssemblySelector {
        Assembly[] assemblies;
        public SpecifiedAssemblySelector(params Assembly[] assemblies) {
            this.assemblies = assemblies;
        }

        public Assembly[] SelectAssemblies() {
            return assemblies;
        }
    }

}