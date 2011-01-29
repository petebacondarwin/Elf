using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Elf.Persistence.Configuration;

namespace Elf.Persistence {
    /// <summary>
    /// Describes the service that provides access to the repository of persistent objects
    /// </summary>
    public interface IRepository : IDisposable {
        /// <summary>
        /// Open a new session for working with persistent objects
        /// </summary>
        /// <returns>A open session</returns>
        ISession OpenSession();
        /// <summary>
        /// Drop and recreate the schema (tables and contraints) in the database
        /// /// </summary>
        void GenerateDatabaseSchema();
        /// <summary>
        /// Attempt to update the schema (tables and contraints) in the database
        /// /// </summary>
        void UpdateDatabaseSchema();
    }

    /// <summary>
    /// Used to access the repository of persistent objects.
    /// </summary>
    public class Repository : IRepository {
        readonly RepositoryConfigurationProvider repositoryConfiguration;
        readonly ISessionFactory sessionFactory;

        public Repository(RepositoryConfigurationProvider repositoryConfiguration) {
            this.repositoryConfiguration = repositoryConfiguration;
            this.sessionFactory = this.repositoryConfiguration.NHConfiguration.BuildSessionFactory();
        }
        public ISession OpenSession() {
            return sessionFactory.OpenSession();
        }
        public void GenerateDatabaseSchema() {
            new SchemaExport(repositoryConfiguration.NHConfiguration).Create(false, true);
        }
        public void UpdateDatabaseSchema() {
            new SchemaUpdate(repositoryConfiguration.NHConfiguration).Execute(false, true);
        }

        public void Dispose() {
            sessionFactory.Dispose();
        }
    }
}
