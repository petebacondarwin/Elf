using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elf.Persistence {
    public class AppDomainAssemblySelector : IAssemblySelector {
        public System.Reflection.Assembly[] SelectAssemblies() {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}
