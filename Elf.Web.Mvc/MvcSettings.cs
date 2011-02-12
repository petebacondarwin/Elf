using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elf.Configuration;

namespace Elf.Web.Mvc {
    public class MvcSettings : BaseSettings {
        public MvcSettings()
            : base() {
            Modules.Add(new MvcModule());
        }
    }
}
