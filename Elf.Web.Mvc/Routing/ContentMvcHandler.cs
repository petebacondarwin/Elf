namespace Elf.Web.Mvc.Routing {
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Our own MvcHandler that can find a controller and action from a <c>ContentRoute</c>
    /// </summary>
    public class ContentMvcHandler : MvcHandler {
        public ContentMvcHandler(RequestContext requestContext)
            : base(requestContext) {
        }

        protected override void ProcessRequest(System.Web.HttpContextBase httpContext) {
            // TODO: Implement the controller/action lookup
            base.ProcessRequest(httpContext);
        }

        protected override IAsyncResult BeginProcessRequest(System.Web.HttpContextBase httpContext, AsyncCallback callback, object state) {
            // TODO: Implement the controller/action lookup
            return base.BeginProcessRequest(httpContext, callback, state);
        }

    }
}
