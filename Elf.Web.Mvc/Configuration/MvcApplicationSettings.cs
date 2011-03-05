namespace Elf.Web.Mvc.Configuration {
    using System.Linq;
    using System.Reflection;
    using System.Web.Compilation;
    using Elf.Configuration;

    /// <summary>
    /// The application settings for an MVC Elf application
    /// </summary>
    public class MvcApplicationSettings : ApplicationSettings {
        public MvcApplicationSettings() : base() {
            // Add the MVC specific services
            Modules.Add(new MvcServices());    
        }

        public virtual void AddWebAppReferencedAssemblies() {
            Assemblies.AddRange(BuildManager.GetReferencedAssemblies().Cast<Assembly>());
        }
    }
}
