namespace Elf.Tests.Web.Mvc {
    using System;
    using System.Reflection;
    using System.Web.Mvc;
    using NUnit.Framework;
    using Elf.Web.Mvc;
    using Elf.Persistence.Entities;
    using Elf.Configuration;

    [TestFixture]
    public class ControllerFinderTests {
        [Test]
        public void FindsPageController() {
            IControllerFinder finder = new ControllerFinder(new AssemblyList() { Assembly.GetExecutingAssembly() });
            Type controllerType = finder.FindControllerFor(new Page());
            Assert.That(controllerType, Is.Not.Null);
            Assert.That(typeof(IController).IsAssignableFrom(controllerType));
            Assert.That(controllerType, Is.EqualTo(typeof(PageController)));

            controllerType = finder.FindControllerFor(new HomePage());
            Assert.That(controllerType, Is.Not.Null);
            Assert.That(typeof(IController).IsAssignableFrom(controllerType));
            Assert.That(controllerType, Is.EqualTo(typeof(PageController)));

            controllerType = finder.FindControllerFor(new ContentItem());
            Assert.That(controllerType, Is.Null);
        }
    }
}