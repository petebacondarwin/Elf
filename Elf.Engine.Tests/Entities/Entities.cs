using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elf.Engine.Tests.Entities {
    public class Page : Elf.Entities.AbstractPage {
        public virtual string BodyText { get; set; }
    }

    public class HomePage : Page {
        public virtual string ExtraInfo { get; set; }
    }
}
