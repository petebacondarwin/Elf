using System;
using FluentNHibernate.Automapping;

namespace Elf.Persistence.Configuration {
    /// <summary>
    /// An Auto Mapping Configuration that selects classes that implement the <see cref="Elf.Entities.IPersistent"/> interface.
    /// </summary>
    public class AutoMappingConfiguration : DefaultAutomappingConfiguration {
        public override bool ShouldMap(Type type) {
            return typeof(Entities.IPersistent).IsAssignableFrom(type);
        }
    }
}
