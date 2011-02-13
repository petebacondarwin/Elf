namespace Elf.Tests.Web.Mvc {
    using System;
    using System.Reflection;
    using System.Web.Mvc;
    using Elf.Web.Mvc;
    using Elf.Persistence.Entities;
    using Elf.Configuration;
    using Xunit;

    public class ControllerFinderTests {
        [Fact]
        public void FindsPageController() {
            IControllerFinder finder = new ControllerFinder(new AssemblyList() { Assembly.GetExecutingAssembly() });
            Type controllerType = finder.FindControllerFor(new Page());
            Assert.NotNull(controllerType);
            Assert.Equal(typeof(PageController), controllerType);

            controllerType = finder.FindControllerFor(new HomePage());
            Assert.NotNull(controllerType);
            Assert.Equal(typeof(PageController),controllerType);

            controllerType = finder.FindControllerFor(new ContentItem());
            Assert.Null(controllerType);
        }
    }
}