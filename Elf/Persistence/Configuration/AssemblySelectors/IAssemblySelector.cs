using System;
using System.Reflection;

namespace Elf.Persistence.Configuration.AssemblySelectors {
    /// <summary>
    /// Describes the services that locates assemblies that may hold types that will be persisted in the repository.
    /// </summary>
    public interface IAssemblySelector {
        Assembly[] SelectAssemblies();
    }
}