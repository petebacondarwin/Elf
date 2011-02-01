using NHibernate.Tool.hbm2ddl;

namespace Elf.Persistence {
    public class SchemaManagement {
        readonly NHibernate.Cfg.Configuration configuration;
        public SchemaManagement(NHibernate.Cfg.Configuration configuration) {
            this.configuration = configuration;
        }
        public void GenerateDatabaseSchema() {
            new SchemaExport(configuration).Create(false, true);
        }
        public void UpdateDatabaseSchema() {
            new SchemaUpdate(configuration).Execute(false, true);
        }
    }
}
