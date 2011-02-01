using System;
using System.Reflection;
using System.Collections.Generic;

namespace Elf.Persistence.Configuration.AssemblySelectors {
    /// <summary>
    /// Provides access to a list of specified assemblies.
    /// </summary>
    /// <remarks>
    /// You can use this locator if you are running in medium trust.
    /// </remarks>
    public class SpecifiedAssemblySelector : NoDynamicAssemblySelector {
        IEnumerable<Assembly> assemblies;
        /// <summary>
        /// Create a new selector with a list of specified assemblies
        /// </summary>
        /// <param name="assemblies">The assemblies we are locating</param>
        public SpecifiedAssemblySelector(params Assembly[] assemblies) {
            this.assemblies = assemblies;
        }
        
        /// <summary>
        /// Provide the list of specified assemblies.
        /// </summary>
        protected override IEnumerable<Assembly> Assemblies {
            get {
                return assemblies;
            }
        }
    }

}