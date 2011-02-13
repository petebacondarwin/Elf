using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Ninject;
using Elf.Tests.Persistence;
using NHibernate;
using Elf.Web.Mvc;
using Elf.Persistence;
using Elf.Web.Mvc.Routing;
using Xunit;

namespace Elf.Tests.Web.Mvc {
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
                    ContentRoute route = new ContentRoute(new ContentFinder(session));

                    // A basic Page is retrieved by path, with default action of "index"
                    var routeData = route.GetRouteData(new StubHttpContextForRouting("~/child"));
                    Assert.NotNull(routeData);
                    Assert.IsType<Page>(routeData.Values["content-item"]);
                    Assert.Equal("child", ((Page)routeData.Values["content-item"]).UrlSegment);
                    Assert.Equal("index", routeData.Values["action"]);

                    // A basic Page is retrieved by path with a specified action "display"
                    routeData = route.GetRouteData(new StubHttpContextForRouting("~/child/display"));
                    Assert.NotNull(routeData);
                    Assert.IsType<Page>(routeData.Values["content-item"]);
                    Assert.Equal("child",((Page)routeData.Values["content-item"]).UrlSegment);
                    Assert.Equal("display", routeData.Values["action"]);

                    // Another page further down the tree is retrieved
                    routeData = route.GetRouteData(new StubHttpContextForRouting("~/child/grand-child"));
                    Assert.NotNull(routeData);
                    Assert.IsType<Page>(routeData.Values["content-item"]);
                    Assert.Equal("grand-child",((Page)routeData.Values["content-item"]).UrlSegment);
                    Assert.Equal("child", ((Page)routeData.Values["content-item"]).Parent.UrlSegment);
                    Assert.Equal("index", routeData.Values["action"]);

                    // More than one extra url segment after a valid content item does not match
                    routeData = route.GetRouteData(new StubHttpContextForRouting("~/child/display/1"));
                    Assert.Null(routeData);

                    // The application root item matches to the HomePage with specified action "moose"
                    routeData = route.GetRouteData(new StubHttpContextForRouting("~/moose"));
                    Assert.NotNull(routeData);
                    Assert.IsType<HomePage>(routeData.Values["content-item"]);
                    Assert.Equal("~",((HomePage)routeData.Values["content-item"]).UrlSegment);
                    Assert.Null(((HomePage)routeData.Values["content-item"]).Parent);
                    Assert.Equal("moose", routeData.Values["action"]);
                }
            }
        }

    }
}