using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elf.Configuration;

namespace Elf.Persistence {
    public class DatabaseConfig : IDatabaseConfig {
        public DatabaseConfig() {
            HibernateProperties = new Dictionary<string, string>();
        }

        public DatabaseConfig(SectionGroup sectionGroup)
            : this(sectionGroup.Database) {
        }

        public DatabaseConfig(DatabaseSection section) : this() {
            Caching = section.Caching;
            CacheProviderClass = section.CacheProviderClass;
            ConnectionStringName = section.ConnectionStringName;
            TablePrefix = section.TablePrefix;
            ChildrenLaziness = section.ChildrenLaziness;
            BatchSize = section.BatchSize;
            Flavour = section.Flavour;
            foreach(System.Configuration.NameValueConfigurationElement item in section.HibernateProperties) {
                HibernateProperties.Add(item.Name, item.Value);
            }
        }
        
        /// <summary>Whether caching should be enabled.</summary>
        public bool Caching { get; set; }
        /// <summary>The cache provider class to use.</summary>
        public string CacheProviderClass { get; set; }
        /// <summary>The name of connection string to use from the .config file connectionStrings section.</summary>
        public string ConnectionStringName { get; set; }
        /// <summary>The prefix used for tables in this site. This can be used to install multiple installations in the same database.</summary>
        public string TablePrefix { get; set; }
        /// <summary>The type of nhibernate laziness to use. Supported values are "true", "false", and "extra".</summary>
        public string ChildrenLaziness { get; set; }
        /// <summary>NHibernate option for database query batching.</summary>
        public int BatchSize { get; set; }
        /// <summary>The database flavour decides which propertes the nhibernate configuration will receive.</summary>
        public DatabaseFlavour Flavour { get; set; }
        /// <summary>Additional NHibernate properties applied after the default flavour-based configuration.</summary>
        public IDictionary<string, string> HibernateProperties { get; set; }
    }
}
