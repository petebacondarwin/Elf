using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Ninject.Web.Mvc;
using Ninject.Activation.Blocks;
using System.Web.Routing;
using Elf.Web.Mvc.Routing;
using Elf.Configuration;

namespace Elf.Web.Mvc {
    public abstract class MvcApplication : NinjectHttpApplication {

        /// <summary>
        /// The kernel scoped to the life of the application.
        /// </summary>
        /// <remarks>
        /// Any items retrieved from this version of the kernel will be scoped to last the lifetime of the application.
        /// </remarks>
        protected IActivationBlock ApplicationScopeKernel;

        /// <summary>
        /// The current settings for the site.
        /// </summary>
        protected ApplicationSettings SiteSettings { get; private set; }

        /// <summary>
        /// Run when the application starts
        /// </summary>
        protected override void OnApplicationStarted() {
            base.OnApplicationStarted();

            // Create an application scope block
            ApplicationScopeKernel = Kernel.BeginBlock();
        }

        /// <summary>
        /// Run when the application stops
        /// </summary>
        protected override void OnApplicationStopped() {
            // Clear up the application 
            ApplicationScopeKernel.Dispose();
        }

        /// <summary>
        /// Register a content route.
        /// </summary>
        /// <param name="routes">The route collection into which the content route is to be registered</param>
        protected virtual void RegisterContentRoute(RouteCollection routes) {
            routes.Add(ApplicationScopeKernel.Get<ContentRoute>());
        }

        /// <summary>
        /// Create a kernel from the site settings
        /// </summary>
        /// <returns></returns>
        protected override IKernel CreateKernel() {
            var kernel = CreateSiteSettings().CreateKernel();
            return kernel;
        }

        /// <summary>
        /// Implement this method to create the settings object for the website application.
        /// </summary>
        /// <returns>An object containing settings for this application.</returns>
        protected abstract Elf.Configuration.ApplicationSettings CreateSiteSettings();
    }
}
