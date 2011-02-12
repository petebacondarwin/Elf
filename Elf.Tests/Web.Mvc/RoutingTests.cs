using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Web;
using Ninject;
using Elf.Tests.Persistence;
using NHibernate;
using Elf.Web.Mvc;
using Elf.Persistence;
using Elf.Web.Mvc.Routing;

namespace Elf.Tests.Web.Mvc {
    [TestFixture]
    public class RoutingTests {
        [Test]
        public void TestVirtualPathMapping() {
            Assert.That(VirtualPathUtility.ToAppRelative("/myapp/one/two/three?q=1&w=2", "/myapp"), Is.EqualTo("~/one/two/three?q=1&w=2"));
        }

        [Test]
        public void TestContentRoute() {
            using (var kernel = TestHelper.CreateKernel()) {
                using (var session = kernel.Get<ISession>()) {
                    DatabaseHelper.GenerateDatabase(session);
                    ContentRoute route = new ContentRoute(new ContentFinder(session), kernel.Get<IControllerFinder>());
                    var routeData = route.GetRouteData(new StubHttpContextForRouting("/", "~/moose"));
                    Assert.That(routeData, Is.Null);

                    routeData = route.GetRouteData(new StubHttpContextForRouting("/", "~/child"));
                    Assert.That(routeData, Is.Not.Null);
                    Assert.That(routeData.Values["content-item"], Is.InstanceOf<Page>());
                    Assert.That(routeData.Values["action"], Is.EqualTo("index"));

                    routeData = route.GetRouteData(new StubHttpContextForRouting("/", "~/child/display"));
                    Assert.That(routeData, Is.Not.Null);
                    Assert.That(routeData.Values["content-item"], Is.InstanceOf<Page>());
                    Assert.That(routeData.Values["action"], Is.EqualTo("display"));

                    routeData = route.GetRouteData(new StubHttpContextForRouting("/", "~/child/grandchild"));
                    Assert.That(routeData, Is.Not.Null);
                    Assert.That(routeData.Values["content-item"], Is.InstanceOf<Page>());
                    Assert.That(routeData.Values["action"], Is.EqualTo("index"));

                    routeData = route.GetRouteData(new StubHttpContextForRouting("/", "~/child/display/1"));
                    Assert.That(routeData, Is.Null);
                }
            }
        }

    }
}