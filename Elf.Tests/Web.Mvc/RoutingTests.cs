namespace Elf.Tests.Web.Mvc {
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Routing;
    using Elf.Persistence;
    using Elf.Tests.Persistence;
    using Elf.Web.Mvc.Routing;
    using Elf.Web.Mvc.Utils;
    using NHibernate;
    using Ninject;
    using Xunit;
    using Elf.Web.Mvc;

    public class RoutingTests {
        [Fact]
        public void VirtualPathUtility_ToAppRelative_RetainsTheQueryString() {
            Assert.Equal(VirtualPathUtility.ToAppRelative("/myapp/one/two/three?q=1&w=2", "/myapp"), "~/one/two/three?q=1&w=2");
        }

        [Fact]
        public void ContentRouteDecodesRoutesCorrectly() {
            using (var kernel = TestHelper.CreateKernel()) {
                using (var session = kernel.Get<ISession>()) {
                    DatabaseHelper.GenerateDatabase(session);
                    ContentRoute route = new ContentRoute(new ContentFinder(session), kernel.Get<IControllerFinder>());

                    // A basic Page is retrieved by path, with default action of "index"
                    var routeData = route.GetRouteData(Mocks.MockupHttpContext("~/child").Object);
                    Assert.NotNull(routeData);
                    Assert.IsType<Page>(routeData.Values.GetContentItem());
                    Assert.Equal("child", routeData.Values.GetContentItem().UrlSegment);
                    Assert.Equal("index", routeData.Values["action"]);

                    // A basic Page is retrieved by path with a specified action "display"
                    routeData = route.GetRouteData(Mocks.MockupHttpContext("~/child/display").Object);
                    Assert.NotNull(routeData);
                    Assert.IsType<Page>(routeData.Values.GetContentItem());
                    Assert.Equal("child",routeData.Values.GetContentItem().UrlSegment);
                    Assert.Equal("display", routeData.Values["action"]);

                    // Another page further down the tree is retrieved
                    routeData = route.GetRouteData(Mocks.MockupHttpContext("~/child/grand-child").Object);
                    Assert.NotNull(routeData);
                    Assert.IsType<Page>(routeData.Values.GetContentItem());
                    Assert.Equal("grand-child",routeData.Values.GetContentItem().UrlSegment);
                    Assert.Equal("child", routeData.Values.GetContentItem().Parent.UrlSegment);
                    Assert.Equal("index", routeData.Values["action"]);

                    // More than one extra url segment after a valid content item does not match
                    routeData = route.GetRouteData(Mocks.MockupHttpContext("~/child/display/1").Object);
                    Assert.Null(routeData);

                    // The application root item matches to the HomePage with specified action "moose"
                    routeData = route.GetRouteData(Mocks.MockupHttpContext("~/moose").Object);
                    Assert.NotNull(routeData);
                    Assert.IsType<HomePage>(routeData.Values.GetContentItem());
                    Assert.Equal("~",routeData.Values.GetContentItem().UrlSegment);
                    Assert.Null(routeData.Values.GetContentItem().Parent);
                    Assert.Equal("moose", routeData.Values["action"]);
                }
            }
        }

        [Fact]
        public void ContentRouteEncodesRoutesCorrectly() {
            using (var kernel = TestHelper.CreateKernel()) {
                using (var session = kernel.Get<ISession>()) {
                    DatabaseHelper.GenerateDatabase(session);
                    ContentRoute route = kernel.Get<ContentRoute>();
                    RequestContext requestContext = new RequestContext(Mocks.MockupHttpContext("/").Object, new RouteData());

                    Page homePage = session.QueryOver<Page>().Where(p=>p.UrlSegment == "~").SingleOrDefault();
                    Page childPage = session.QueryOver<Page>().Where(p => p.UrlSegment == "child").SingleOrDefault();
                    Page grandChildPage = childPage.Children.OfType<Page>().FirstOrDefault();
                    RouteValueDictionary dict = new RouteValueDictionary();

                    Assert.Null(route.GetVirtualPath(requestContext, null));

                    Assert.Throws<ArgumentException>(
                        // Invalid RouteData in the context
                        () => route.GetVirtualPath(new RequestContext(), new RouteValueDictionary())
                    );
                    
                    // The root home page (with default action) Url is "~"
                    dict.Remove("action");
                    dict["content-item"] = homePage;
                    var path = route.GetVirtualPath(requestContext, dict);
                    Assert.NotNull(path);
                    Assert.Equal("~", path.VirtualPath);

                    // A child page (with default action) Url is "~/child"
                    dict.Remove("action");
                    dict["content-item"] = childPage;
                    path = route.GetVirtualPath(requestContext, dict);
                    Assert.NotNull(path);
                    Assert.Equal("~/child", path.VirtualPath);

                    // A grand child page (with default action) Url is "~/child/grand-child"
                    dict.Remove("action");
                    dict["content-item"] = grandChildPage;
                    path = route.GetVirtualPath(requestContext, dict);
                    Assert.NotNull(path);
                    Assert.Equal("~/child/grand-child", path.VirtualPath);

                    // The root home page (with specific default action) Url is "~"
                    dict.Remove("action");
                    dict["action"] = "index";
                    dict["content-item"] = homePage;
                    path = route.GetVirtualPath(requestContext, dict);
                    Assert.NotNull(path);
                    Assert.Equal("~", path.VirtualPath);

                    // A child page (with default action) Url is "~/child"
                    dict["action"] = "index";
                    dict["content-item"] = childPage;
                    path = route.GetVirtualPath(requestContext, dict);
                    Assert.NotNull(path);
                    Assert.Equal("~/child", path.VirtualPath);

                    // A grand child page (with default action) Url is "~/child/grand-child"
                    dict["action"] = "index";
                    dict["content-item"] = grandChildPage;
                    path = route.GetVirtualPath(requestContext, dict);
                    Assert.NotNull(path);
                    Assert.Equal("~/child/grand-child", path.VirtualPath);

                    // The root home page (with non-default action "display") Url is "~/display"
                    dict["action"] = "display";
                    dict["content-item"] = homePage;
                    path = route.GetVirtualPath(requestContext, dict);
                    Assert.NotNull(path);
                    Assert.Equal("~/display", path.VirtualPath);

                    // A child page (with non-default action "display") Url is "~/child/display"
                    dict["action"] = "display";
                    dict["content-item"] = childPage;
                    path = route.GetVirtualPath(requestContext, dict);
                    Assert.NotNull(path);
                    Assert.Equal("~/child/display", path.VirtualPath);

                    // A grand child page (with non-default action "display") Url is "~/child/grand-child/display"
                    dict["action"] = "display";
                    dict["content-item"] = grandChildPage;
                    path = route.GetVirtualPath(requestContext, dict);
                    Assert.NotNull(path);
                    Assert.Equal("~/child/grand-child/display", path.VirtualPath);

                }
            }
        }
    }
}