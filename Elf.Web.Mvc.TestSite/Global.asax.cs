using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Elf.Persistence;
using Elf.Web.Mvc.Routing;
using Elf.Web.Mvc.TestSite.Modules.Main;
using Ninject.Parameters;

namespace Elf.Web.Mvc.TestSite {
    /// <summary>
    /// The web application root object.
    /// </summary>
    /// <remarks>
    /// Note that this class inherits from <c>Ninject.Web.Mvc.NinjectHttpApplication</c>,
    /// which automatically injects dependencies into the controllers and views as required.
    /// </remarks>
    public class MvcApplication : Ninject.Web.Mvc.NinjectHttpApplication {
        /// <summary>
        /// The scope of the content route - this stops the session from being disposed before we are ready
        /// </summary>
        Ninject.Activation.Blocks.IActivationBlock ScopeBlock;
        
        public void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            ScopeBlock = Kernel.BeginBlock();
            routes.Add(ScopeBlock.Get<ContentRoute>());

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected override void OnApplicationStarted() {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }

        protected override Ninject.IKernel CreateKernel() {
            var kernel = new SiteSettings().CreateKernel();

            // Load all the modules in this website
            kernel.Load(System.Reflection.Assembly.GetExecutingAssembly());
            return kernel;
        }

        protected override void OnApplicationStopped() {
            ScopeBlock.Dispose();
        }
    }
}