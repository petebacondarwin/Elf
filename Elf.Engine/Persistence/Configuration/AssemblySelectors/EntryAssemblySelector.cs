using System;
using System.Reflection;

namespace Elf.Persistence.Configuration.AssemblySelectors {
    /// <summary>
    /// Locates the entry assembly.
    /// </summary>
    public class EntryAssemblySelector : IAssemblySelector {
        public Assembly[] SelectAssemblies() {
            return new Assembly[] { Assembly.GetEntryAssembly() };
        }
    }
}