using System;
using System.Reflection;

namespace Elf.Persistence.Configuration.AssemblySelectors {
    /// <summary>
    /// Locates all the assemblies in the current AppDomain.
    /// </summary>
    public class AppDomainAssemblySelector : IAssemblySelector {
        public Assembly[] SelectAssemblies() {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}