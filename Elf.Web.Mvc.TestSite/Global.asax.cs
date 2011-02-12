using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Elf.Persistence;
using Elf.Web.Mvc.Routing;

namespace Elf.Web.Mvc.TestSite {
    /// <summary>
    /// The web application root object.
    /// </summary>
    /// <remarks>
    /// Note that this class inherits from <c>Ninject.Web.Mvc.NinjectHttpApplication</c>,
    /// which automatically injects dependencies into the controllers and views as required.
    /// </remarks>
    public class MvcApplication : Ninject.Web.Mvc.NinjectHttpApplication {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
        }

        public void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.Add(new ContentRoute(Kernel.Get<IContentFinder>(), Kernel.Get<IControllerFinder>()));
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected override void OnApplicationStarted() {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected override Ninject.IKernel CreateKernel() {
            var kernel = new StandardKernel();

            // Load all the modules in this website
            kernel.Load(System.Reflection.Assembly.GetExecutingAssembly());
            return kernel;
        }
    }
}