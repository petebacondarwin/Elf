namespace Elf.Web.Mvc.Routing {
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Mvc.Async;
    using Elf.Web.Mvc.Utils;
    using System.Web;
    using System.Web.SessionState;
    using Elf.Persistence.Entities;

    /// <summary>
    /// Our own MvcHandler that can find a controller and action from a <c>ContentRoute</c>
    /// </summary>
    public class ContentHttpHandler : MvcHandler {

        public ContentHttpHandler(RequestContext requestContext, IControllerFinder controllerFinder)
            : base(requestContext) {

            // The main point of this class is that we are going to find the correct controller from the ContentItem
            // that is passed in the RouteData from the ContentRoute.  So we find it now.
            ContentItem item = RequestContext.RouteData.Values.GetContentItem();

            if (item == null) {
                throw new ArgumentException("requestContext must contain a valid Content Item", "requestContext");
            }
            string controllerName = controllerFinder.FindControllerNameFor(item);

            // MVC expects the controller to be named in the RouteData so add that now that we know it
            requestContext.RouteData.Values["controller"] = controllerName;
            
            // Also MVC allows the controller to modify the session state behavior via an attribute so arrange that now
            // (Normally MVC does this inside the MvcRouteHandler but we didn't know what controller we were going to use at that stage)
            requestContext.HttpContext.SetSessionStateBehavior(GetSessionStateBehavior(controllerName));
        }

        private SessionStateBehavior GetSessionStateBehavior(string controllerName) {
            IControllerFactory controllerFactory = ControllerBuilder.Current.GetControllerFactory();
            return controllerFactory.GetControllerSessionBehavior(RequestContext, controllerName);
        }
    }
}
