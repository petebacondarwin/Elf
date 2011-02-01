using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Elf.Persistence.Configuration.AssemblySelectors {
    /// <summary>
    /// Locates all the assemblies in the current AppDomain.
    /// </summary>
    public class AppDomainAssemblySelector : NoDynamicAssemblySelector {
        /// <summary>
        /// Get all the assemblies loaded in the current app domain
        /// </summary>
        protected override IEnumerable<Assembly> Assemblies {
            get {
                return AppDomain.CurrentDomain.GetAssemblies();
            }
        }
    }
}