using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Elf.Web.Mvc.Routing;
using System.Web.Routing;
using System.Web;
using Elf.Web.Mvc;
using Elf.Web.Mvc.Utils;

namespace Elf.Tests.Web.Mvc {
    public class ContentRouteHandlerTests {
        [Fact]
        public void GetsAContentHttpHandler() {
            IRouteHandler routeHandler = new ContentRouteHandler(new ControllerFinder(new Configuration.AssemblyList() { GetType().Assembly}));
            RequestContext requestContext = new RequestContext(new StubHttpContextForRouting(),new RouteData());
            requestContext.RouteData.Values.SetContentItem(new Elf.Persistence.Entities.ContentItem());
            IHttpHandler handler = routeHandler.GetHttpHandler(requestContext);
            Assert.IsType<ContentHttpHandler>(handler);
        }

        [Fact]
        public void MustHaveAContentItemInRequestContext() {
            IRouteHandler routeHandler = new ContentRouteHandler(new ControllerFinder(new Configuration.AssemblyList() { GetType().Assembly }));
            RequestContext requestContext = new RequestContext(new StubHttpContextForRouting(), new RouteData());
            
            Assert.Throws<ArgumentException>( () => routeHandler.GetHttpHandler(requestContext) );
        }
    }
}
