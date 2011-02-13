using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elf.Configuration;
using System.Web.Compilation;
using System.Reflection;

namespace Elf.Web.Mvc {
    public class MvcSettings : BaseSettings {
        public MvcSettings() : base() {
            // Add the MVC specific Ninject bindings
            Modules.Add(new MvcModule());    
        }

        public virtual void AddWebAppReferencedAssemblies() {
            Assemblies.AddRange(BuildManager.GetReferencedAssemblies().Cast<Assembly>());
        }
    }
}
