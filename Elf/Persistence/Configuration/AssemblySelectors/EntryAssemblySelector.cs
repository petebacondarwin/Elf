using System;
using System.Collections.Generic;
using System.Reflection;

namespace Elf.Persistence.Configuration.AssemblySelectors {
    /// <summary>
    /// Locates the entry assembly.
    /// </summary>
    public class EntryAssemblySelector : NoDynamicAssemblySelector {
        /// <summary>
        /// Get the start assembly for this app domain
        /// </summary>
        /// <remarks>
        /// This is probably most useful in a windows or console application
        /// </remarks>
        protected override IEnumerable<Assembly> Assemblies {
            get {
                return new Assembly[] { Assembly.GetEntryAssembly() };
            }
        }
    }
}