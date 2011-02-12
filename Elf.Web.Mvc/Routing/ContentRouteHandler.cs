namespace Elf.Web.Mvc.Routing {
    using System.Web.Routing;
    using System.Web.SessionState;
    using System.Web.Mvc;
    using System.Web;

    /// <summary>
    /// Our own route handler that uses our own MvcHandler instead of the built-in one.
    /// </summary>
    public class ContentRouteHandler : MvcRouteHandler {
        public ContentRouteHandler() : base() {
        }

        public ContentRouteHandler(IControllerFactory controllerFactory) : base(controllerFactory) {
        }

        protected override IHttpHandler GetHttpHandler(RequestContext requestContext) {
            requestContext.HttpContext.SetSessionStateBehavior(GetSessionStateBehavior(requestContext));
            return new ContentMvcHandler(requestContext);
        }
    }
}
