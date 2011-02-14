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
using System.Web.Mvc;
using System.Web.SessionState;

namespace Elf.Tests.Web.Mvc {
    public class ContentRouteHandlerTests {
        #region Mocks
        class MockControllerFactory : IControllerFactory {
            public IController CreateController(RequestContext requestContext, string controllerName) {
                return new PageController();
            }

            public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName) {
                return SessionStateBehavior.Default;
            }

            public void ReleaseController(IController controller) {
                // Do nothing
            }
        }
#endregion Mocks

        [Fact]
        public void GetsAContentHttpHandler() {
            ControllerBuilder.Current.SetControllerFactory(new MockControllerFactory());
            RequestContext requestContext = new RequestContext(new StubHttpContextForRouting(),new RouteData());
            requestContext.RouteData.Values.SetContentItem(new Page());
            IRouteHandler routeHandler = new ContentRouteHandler(new ControllerFinder(new Configuration.AssemblyList() { GetType().Assembly }));

            IHttpHandler handler = routeHandler.GetHttpHandler(requestContext);
            
            Assert.IsType<MvcHandler>(handler);
            Assert.NotNull(((MvcHandler)handler).RequestContext.RouteData.Values.GetContentItem());
            Assert.Equal("Page", ((MvcHandler)handler).RequestContext.RouteData.Values["controller"]);
        }

        [Fact]
        public void MustHaveAContentItemInRequestContext() {
            IRouteHandler routeHandler = new ContentRouteHandler(new ControllerFinder(new Configuration.AssemblyList() { GetType().Assembly }));
            RequestContext requestContext = new RequestContext(new StubHttpContextForRouting(), new RouteData());
            
            Assert.Throws<ArgumentException>( () => routeHandler.GetHttpHandler(requestContext) );
        }
    }
}
