using System;
using FluentNHibernate.Automapping;

namespace Elf.Persistence {
    public interface IPersistenceModelProvider {
        AutoPersistenceModel GetPersistenceModel();
    }
}
