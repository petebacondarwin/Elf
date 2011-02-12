using NHibernate.Tool.hbm2ddl;
using NHibernate;

namespace Elf.Persistence {
    /// <summary>
    /// Manage the database schema.
    /// </summary>
    /// <remarks>
    /// Use this class to generate from scratch or update the database to comply with the mapped entity classes.</remarks>
    public class SchemaManager {
        readonly NHibernate.Cfg.Configuration configuration;

        /// <summary>
        /// Create a new SchemaManagement instance, using the given NHibernate configuration.
        /// </summary>
        /// <param name="configuration">The configuration from which to generate or update the database schema.</param>
        public SchemaManager(NHibernate.Cfg.Configuration configuration) {
            this.configuration = configuration;
        }

        /// <summary>
        /// Generate the database from the mapped entity classes.
        /// </summary>
        public void GenerateDatabaseSchema() {
            SchemaExport schema = new SchemaExport(configuration);
            schema.Execute(false, true, false);
        }

        /// <summary>
        /// Generate the database from the mapped entity classes, using the connection from the given session
        /// </summary>
        /// <param name="session">The session from which to get the connection</param>
        /// <remarks>
        /// This overload is useful for InMemory SQLite databases as the database is only good
        /// for the life of the session.  So we need to use the same session for creating the 
        /// schema as we are using for the rest of the database operations.
        /// </remarks>
        public void GenerateDatabaseSchema(ISession session) {
            SchemaExport schema = new SchemaExport(configuration);
            schema.Execute(false, true, false, session.Connection, null);
        }

        /// <summary>
        /// Update the database as best NHibernate can.
        /// </summary>
        public void UpdateDatabaseSchema() {
            SchemaUpdate schema = new SchemaUpdate(configuration);
            schema.Execute(false, true);
        }
    }
}
