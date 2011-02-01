using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elf.Persistence.Configuration.AssemblySelectors {
    /// <summary>
    /// A base assembly selector that ensure that no dynamic assemblies are selected
    /// </summary>
    public abstract class NoDynamicAssemblySelector : IAssemblySelector {
        /// <summary>
        /// Returns the list of non-dynamic assemblies
        /// </summary>
        /// <returns>An array of assemblies</returns>
        public System.Reflection.Assembly[] SelectAssemblies() {
            // Ensure that no dynamic assemblies are selected as the automapping does not like them
            return Assemblies.Where(assembly=>!assembly.IsDynamic).ToArray();
        }

        /// <summary>
        /// Implement this method to provide the list of assemblies
        /// </summary>
        protected abstract IEnumerable<System.Reflection.Assembly> Assemblies { get; }
    }
}
