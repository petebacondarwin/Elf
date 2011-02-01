using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Elf.Persistence.Modules;
using FluentNHibernate.Cfg.Db;
using Ninject.Activation;

namespace Elf.Web.Mvc.TestSite {
    public class ElfModule : CoreModule {
        public override IPersistenceConfigurer GetDatabaseConfiguration(IContext context) {
            return SQLiteConfiguration.Standard.UsingFile("|DataDirectory|\\content.db");
        }
    }
}