namespace Elf.Web.Mvc.Routing {
    using System.Web.Routing;
    using System.Web.SessionState;
    using System.Web.Mvc;
    using System.Web;

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

            // We don't know what the controller is yet.
            // So we can't call this here, since GetSessionStateBehavior uses IControllerFactory.GetControllerSessionBehavior.
            // => requestContext.HttpContext.SetSessionStateBehavior(GetSessionStateBehavior(requestContext));
            // We'll do it inside the ContentHttpHandler later instead
            return new ContentHttpHandler(requestContext, controllerFinder);
        }
    }
}
