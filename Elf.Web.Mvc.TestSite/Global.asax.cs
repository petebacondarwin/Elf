using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Elf.Persistence;
using Elf.Web.Mvc.Routing;
using Elf.Web.Mvc.TestSite.Areas.Main;
using Ninject.Parameters;
using FluentNHibernate.Cfg.Db;
using Elf.Web.Mvc.TestSite.Areas.Main.Models;
using System.Reflection;

namespace Elf.Web.Mvc.TestSite {
    /// <summary>
    /// The web application root object.
    /// </summary>
    /// <remarks>
    /// Note that this class inherits from <c>Ninject.Web.Mvc.NinjectHttpApplication</c>,
    /// which automatically injects dependencies into the controllers and views as required.
    /// </remarks>
    public class MvcApplication : Elf.Web.Mvc.MvcApplication {
        
        public void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RegisterContentRoute(routes);

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected override void OnApplicationStarted() {
            base.OnApplicationStarted();

            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
        }


        protected override Elf.Configuration.ApplicationSettings CreateSiteSettings() {
            var settings = new Configuration.MvcApplicationSettings();
            settings.DatabaseSettings = SQLiteConfiguration.Standard.UsingFile("|DataDirectory|\\content.db");
            settings.AddWebAppReferencedAssemblies();
            return settings;
        }
    }
}