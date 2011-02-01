using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Elf.Persistence.Configuration.AssemblySelectors;

namespace Elf.Tests.Persistence {
    [TestFixture]
    public class AssemblySelectorTests {
        [Test]
        public void TestAppDomainSelector() {
            var selector = new AppDomainAssemblySelector();
            var assemblies = selector.SelectAssemblies();
            Assert.That(assemblies.Any(assembly => assembly.IsDynamic), Is.Not.True);
        }
    }
}
