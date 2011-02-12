namespace Elf.Web.Mvc.TestSite.Modules.Main {
    using Elf.Web.Mvc.TestSite.Modules.Main.Models;
    using FluentNHibernate.Cfg.Db;

    /// <summary>
    /// We override the core module to establish our own database configuration
    /// </summary>
    public class SiteSettings : MvcSettings {
        public SiteSettings() : base() {
            // Change the database
            DatabaseSettings = SQLiteConfiguration.Standard.UsingFile("|DataDirectory|\\content.db");
            Assemblies.Add(System.Reflection.Assembly.GetAssembly(typeof(Page)));
        }
    }
}