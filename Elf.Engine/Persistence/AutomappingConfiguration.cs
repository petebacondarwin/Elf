﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;

namespace Elf.Persistence {
    public class AutomappingConfiguration : DefaultAutomappingConfiguration {
        public override bool ShouldMap(Type type) {
            return typeof(Entities.IPersistent).IsAssignableFrom(type);
        }
    }
}
