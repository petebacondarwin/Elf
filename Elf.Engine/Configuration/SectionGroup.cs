using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elf.Configuration {
    public class SectionGroup : System.Configuration.ConfigurationSectionGroup {
        public DatabaseSection Database {
            get { return (DatabaseSection)Sections["database"]; }
        }
    }
}
