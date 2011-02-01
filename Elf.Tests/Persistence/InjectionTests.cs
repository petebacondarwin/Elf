using System.Linq;
using Elf.Persistence.Configuration;
using Elf.Persistence.Configuration.AssemblySelectors;
using Elf.Tests.Persistence;
using FluentNHibernate.Automapping;
using NHibernate;
using Ninject;
using NUnit.Framework;

namespace Elf.Tests {
    [TestFixture]
    public class InjectionTests {
        [Test]
        public void TestElfBindings() {
            using (var kernel = new StandardKernel(new TestModule())) {
                var selector = kernel.Get<IAssemblySelector>();
                Assert.That(selector, Is.InstanceOf<AppDomainAssemblySelector>());

                var mappingConfig = kernel.Get<IAutomappingConfiguration>();
                Assert.That(mappingConfig, Is.InstanceOf<AutoMappingConfiguration>());

                var NHConfig = kernel.Get<NHibernate.Cfg.Configuration>();
                Assert.That(NHConfig, Is.InstanceOf<NHibernate.Cfg.Configuration>());

                var persistenceModel = kernel.Get<AutoPersistenceModel>();
                Assert.That(persistenceModel, Is.InstanceOf<AutoPersistenceModel>());
                Assert.That(persistenceModel.Conventions.Find<FluentNHibernate.Conventions.IConvention>().Count(), Is.EqualTo(2));

                var sessionFactory = kernel.Get<ISessionFactory>();
                Assert.That(sessionFactory, Is.Not.Null);
                Assert.That(sessionFactory.GetClassMetadata(typeof(Elf.Persistence.Entities.ContentItem)), Is.Not.Null);

                var session = kernel.Get<ISession>();
                Assert.That(session, Is.Not.Null);
            }
        }
    }
}
