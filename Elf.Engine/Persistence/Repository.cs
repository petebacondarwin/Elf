using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Elf.Persistence {
    public class Repository : IRepository {
        IRepositoryConfiguration repositoryConfiguration;
        ISessionFactory sessionFactory;

        public Repository(IRepositoryConfiguration repositoryConfiguration) {
            this.repositoryConfiguration = repositoryConfiguration;
            this.sessionFactory = this.repositoryConfiguration.NHConfiguration.BuildSessionFactory();
        }
        public ISession OpenSession() {
            return sessionFactory.OpenSession();
        }
        public void GenerateDatabaseSchema() {
            new SchemaExport(repositoryConfiguration.NHConfiguration).Create(true, true);
        }
        public void UpdateDatabaseSchema() {
            new SchemaUpdate(repositoryConfiguration.NHConfiguration).Execute(true, true);
        }
    }
}
