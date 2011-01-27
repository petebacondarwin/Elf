using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Elf.Persistence {
    public interface IAssemblySelector {
        Assembly[] SelectAssemblies();
    }
}
