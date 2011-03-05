using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Elf.Web.Mvc;
using Elf.Persistence.Entities;

namespace Elf.Tests {
    public class PageController : AbstractController<Page>{
    }

    public class AbstractController<T> : ContentController<T> where T : AbstractPage {
    }
}
