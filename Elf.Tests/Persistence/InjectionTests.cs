namespace Elf.Tests {
    using System.Linq;
    using Elf.Configuration;
    using Elf.Persistence.Configuration;
    using FluentNHibernate.Automapping;
    using NHibernate;
    using Ninject;
    using Xunit;

    public class InjectionTests {
        [Fact]
        public void TestElfBindings() {
            using (var kernel = TestHelper.CreateKernel()) {
                var assemblies = kernel.Get<AssemblyList>();
                Assert.Equal(2, assemblies.Count);

                var mappingConfig = kernel.Get<IAutomappingConfiguration>();
                Assert.IsType<AutoMappingConfiguration>(mappingConfig);

                var NHConfig = kernel.Get<NHibernate.Cfg.Configuration>();
                Assert.IsType<NHibernate.Cfg.Configuration>(NHConfig);

                var persistenceModel = kernel.Get<AutoPersistenceModel>();
                Assert.IsType<AutoPersistenceModel>(persistenceModel);
                Assert.Equal(2, persistenceModel.Conventions.Find<FluentNHibernate.Conventions.IConvention>().Count());

                var sessionFactory = kernel.Get<ISessionFactory>();
                Assert.NotNull(sessionFactory);
                Assert.NotNull(sessionFactory.GetClassMetadata(typeof(Elf.Persistence.Entities.ContentItem)));

                var session = kernel.Get<ISession>();
                Assert.NotNull(session);
            }
        }
    }
}
