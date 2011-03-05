namespace Elf.Web.Mvc.Routing {
    using System;
    using System.Web.Routing;
    using Elf.Persistence;
    using Elf.Persistence.Entities;
    using Elf.Web.Mvc.Utils;

    /// <summary>
    /// A route that matches urls to content items.  See <see cref="ContentRoute.GetRouteData" /> for more information.
    /// </summary>
    public class ContentRoute : System.Web.Routing.RouteBase {
        readonly IContentFinder contentFinder;
        readonly IRouteHandler handler;
        public ContentRoute(IContentFinder contentFinder, ContentRouteHandler handler) {
            this.contentFinder = contentFinder;
            this.handler = handler;
        }

        /// <summary>
        /// Match the specified url to a Content Item.
        /// </summary>
        /// <param name="httpContext">The context of the current request upon which to match</param>
        /// <returns>A routedata object if the route matched or null if not</returns>
        /// <remarks>
        /// If matched the route data will contain two items:
        /// - "content-item" => The item that corresponds to the url
        /// - "action" => An action to perform against that item, the default being index
        /// </remarks>
        public override RouteData GetRouteData(System.Web.HttpContextBase httpContext) {
            // Get the current url mapped relative to the application.
            // This should be of the form ~/segment1/segment2
            string url = httpContext.Request.AppRelativeCurrentExecutionFilePath;

            // index is the default action if none is specified
            string action = "index";
            ContentItem item = contentFinder.Find(url);
            if (item == null) {
                // The full url didn't match so we try removing the last segment as this might be an action
                int index = url.LastIndexOf('/');
                action = url.Substring(index + 1);
                url = url.Remove(index);
                item = contentFinder.Find(url);
            }
            if (item != null) {
                // We have found a content item so return this and the action route data
                RouteData data = new RouteData(this, handler);
                data.Values.SetContentItem(item);
                data.Values["action"] = action;
                return data;
            }
            // We didn't find a content item for this url
            return null;
        }

        /// <summary>
        /// Map route data back to a url
        /// </summary>
        /// <param name="requestContext">The context of the request to map</param>
        /// <param name="values">The information passed to the route with which it will attempt to return a virtual path</param>
        /// <returns>The VirtualPathData for the mapped url or null if there was no match</returns>
        public override System.Web.Routing.VirtualPathData GetVirtualPath(System.Web.Routing.RequestContext requestContext, System.Web.Routing.RouteValueDictionary values) {
            if (requestContext.RouteData == null) {
                throw new ArgumentException("requestContext must have a non-null RouteData object", "requestContext");
            }

            // Setup default values if necessary
            RouteValueDictionary currentValues = requestContext.RouteData.Values ?? new RouteValueDictionary();
            values = values ?? new RouteValueDictionary();

            // Extract the content item from either the values or if not there then the values in the requestContext
            ContentItem item = values.GetContentItem() ?? currentValues.GetContentItem();
            if ( item == null ) {
                // There is no content item to match
                return null;
            }

            // Build the url from each ContentItem's url segment
            string url = item.UrlSegment;
            while (item.Parent != null) {
                item = item.Parent;
                url = item.UrlSegment + "/" + url;
            }

            // Add on an action segment if the action is not the default
            string action = values["action"] as String ?? "index";
            if (action != "index") {
                url = url + "/" + action;
            }
            return new VirtualPathData(this, url);
        }
    }
}
