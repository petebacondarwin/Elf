using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elf.Persistence {
    /// <summary>
    /// Classes implementing this provide the configuration for NHibernate.
    /// </summary>
    public interface IDatabaseConfig {
        /// <summary>Whether caching should be enabled.</summary>
        bool Caching { get; set; }
        /// <summary>The cache provider class to use.</summary>
        string CacheProviderClass { get; set; }
        /// <summary>The name of connection string to use from the .config file connectionStrings section.</summary>
        string ConnectionStringName { get; set; }
        /// <summary>The prefix used for tables in this site. This can be used to install multiple installations in the same database.</summary>
        string TablePrefix { get; set; }
        /// <summary>The type of nhibernate laziness to use. Supported values are "true", "false", and "extra".</summary>
        string ChildrenLaziness { get; set; }
        /// <summary>NHibernate option for database query batching.</summary>
        int BatchSize { get; set; }
        /// <summary>The database flavour decides which propertes the nhibernate configuration will receive.</summary>
        DatabaseFlavour Flavour { get; set; }
        /// <summary>Additional NHibernate properties applied after the default flavour-based configuration.</summary>
        IDictionary<string, string> HibernateProperties { get; }
    }
}