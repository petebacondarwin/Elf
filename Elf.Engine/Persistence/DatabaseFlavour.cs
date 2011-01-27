﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elf.Persistence {
    public enum DatabaseFlavour {
        SqlServer2008,
        SqlServer2005,
        SqlServer2000,
        SqlCe,
        MySql,
        SqLite,
        Firebird,
        Generic,
        Jet,
        DB2,
        Oracle9i,
        Oracle10g,
        AutoDetect
    }
}