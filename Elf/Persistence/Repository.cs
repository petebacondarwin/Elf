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
        /// Gets the current persistence session
        /// </summary>
        ISession CurrentSession { get; }

        /// <summary>
        /// Begin a transaction wrapped session and store it in the CurrentSessionContext
        /// </summary>
        void BeginSession();
        /// <summary>
        /// End a session, committing the transaction if necessary removing it from the CurrentSessionContext
        /// </summary>
        void EndSession();
        
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

        /// <summary>
        /// Get the current persistence session
        /// </summary>
        public ISession CurrentSession {
            get {
                return sessionFactory.GetCurrentSession();
            }
        }

        /// <summary>
        /// Begin a transaction wrapped session and store it in the CurrentSessionContext
        /// </summary>
        public void BeginSession() {
            ISession session = sessionFactory.OpenSession();
            session.BeginTransaction();
            NHibernate.Context.CurrentSessionContext.Bind(session);
        }

        /// <summary>
        /// End a session, committing the transaction if necessary removing it from the CurrentSessionContext
        /// </summary>
        public void EndSession() {
            ISession session = NHibernate.Context.CurrentSessionContext.Unbind(sessionFactory);
            if (session != null) {
                if (session.Transaction != null) {
                    if (session.Transaction.IsActive) {
                        session.Transaction.Commit();
                    }
                }
            }
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
