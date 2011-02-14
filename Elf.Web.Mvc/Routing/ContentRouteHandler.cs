namespace Elf.Web.Mvc.Routing {
    using System.Web.Routing;
    using System.Web.SessionState;
    using System.Web.Mvc;
    using System.Web;
    using Elf.Persistence.Entities;
    using Elf.Web.Mvc.Utils;
    using System;

    /// <summary>
    /// Our own route handler that uses our own MvcHandler instead of the built-in one.
    /// </summary>
    public class ContentRouteHandler : MvcRouteHandler {
        readonly IControllerFinder controllerFinder;
        /// <summary>
        /// Create a new default content route handler
        /// </summary>
        public ContentRouteHandler(IControllerFinder controllerFinder) : base() {
            this.controllerFinder = controllerFinder;
        }

        /// <summary>
        /// Create a content route handler with the provided controller factory
        /// </summary>
        /// <param name="controllerFactory">The object that will create our controllers for us.</param>
        public ContentRouteHandler(IControllerFinder controllerFinder, IControllerFactory controllerFactory) : base(controllerFactory) {
            this.controllerFinder = controllerFinder;
        }

        /// <summary>
        /// Get a Http Hander for the given request 
        /// </summary>
        /// <param name="requestContext"></param>
        /// <returns></returns>
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext) {
            // Get the content item that the route found for us
            ContentItem item = requestContext.RouteData.Values.GetContentItem();
            if (item == null) {
                throw new ArgumentException("requestContext must contain a valid Content Item", "requestContext");
            }

            // The main point of this class is that we are going to find the correct controller from the current ContentItem
            string controllerName = controllerFinder.FindControllerNameFor(item);

            // MVC expects the controller to be named in the RouteData so add that now that we know it
            requestContext.RouteData.Values["controller"] = controllerName;
            requestContext.HttpContext.SetSessionStateBehavior(GetSessionStateBehavior(requestContext));

            return new MvcHandler(requestContext);
        }
    }
}
