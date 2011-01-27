using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Elf.Configuration {
    /// <summary>
    /// Database configuration section for nhibernate database connection.
    /// </summary>
    public class DatabaseSection : ConfigurationSection {
        /// <summary>Whether cacheing should be enabled.</summary>
        [ConfigurationProperty("caching", DefaultValue = false)]
        public bool Caching {
            get { return (bool)base["caching"]; }
            set { base["caching"] = value; }
        }

        /// <summary>The nhibernate cache provider class to use.</summary>
        /// <remarks>
        /// Other cache providers:
        /// NHibernate.Cache.NoCacheProvider, NHibernate
        /// NHibernate.Caches.SysCache2.SysCacheProvider,NHibernate.Caches.SysCache2
        /// </remarks>
        [ConfigurationProperty("cacheProviderClass", DefaultValue = "NHibernate.Cache.NoCacheProvider, NHibernate")]
        public string CacheProviderClass {
            get { return (string)base["cacheProviderClass"]; }
            set { base["cacheProviderClass"] = value; }
        }

        /// <summary>The connection string to pick amont the connection strings in the connectionStrings section.</summary>
        [ConfigurationProperty("connectionStringName", DefaultValue = "ElfDB")]
        public string ConnectionStringName {
            get { return (string)base["connectionStringName"]; }
            set { base["connectionStringName"] = value; }
        }

        /// <summary>The prefix used for tables in this site. This can be used to install multiple installations in the same database.</summary>
        [ConfigurationProperty("tablePrefix", DefaultValue = "")]
        public string TablePrefix {
            get { return (string)base["tablePrefix"]; }
            set { base["tablePrefix"] = value; }
        }

        /// <summary>The type of nhibernate laziness to use. Supported values are "true", "false", and "extra".</summary>
        [ConfigurationProperty("childrenLaziness", DefaultValue = "extra")]
        public string ChildrenLaziness {
            get { return (string)base["childrenLaziness"]; }
            set { base["childrenLaziness"] = value; }
        }

        /// <summary>NHibernate option for database query batching.</summary>
        [ConfigurationProperty("batchSize", DefaultValue = 25)]
        public int BatchSize {
            get { return (int)base["batchSize"]; }
            set { base["batchSize"] = value; }
        }

        /// <summary>The database flavour decides which propertes the nhibernate configuration will receive.</summary>
        [ConfigurationProperty("flavour", DefaultValue = Persistence.DatabaseFlavour.AutoDetect)]
        public Persistence.DatabaseFlavour Flavour {
            get { return (Persistence.DatabaseFlavour)base["flavour"]; }
            set { base["flavour"] = value; }
        }

        /// <summary>Additional nhibernate properties applied after the default flavour-based configuration.</summary>
        [ConfigurationProperty("hibernateProperties")]
        public NameValueConfigurationCollection HibernateProperties {
            get { return (NameValueConfigurationCollection)base["hibernateProperties"]; }
            set { base["hibernateProperties"] = value; }
        }
    }
}
