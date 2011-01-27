using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Elf.Persistence {
    public interface IRepository {
        ISession OpenSession();
        void GenerateDatabaseSchema();
        void UpdateDatabaseSchema();
    }
}
