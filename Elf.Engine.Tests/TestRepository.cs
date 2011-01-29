using Elf.Tests.Entities;
using Elf.Persistence;
using Elf.Persistence.Configuration;
using NUnit.Framework;
using Elf.Persistence.Configuration.AssemblySelectors;
using Elf.Persistence.Configuration.AutoMapping;

namespace Elf.Tests {
    [SetUpFixture]
    public class TestRepository {
        static IRepository repository;
            
        [SetUp]
        public void Create() {
            repository = new Repository(
                new RepositoryConfigurationProvider(
                    FluentNHibernate.Cfg.Db.SQLiteConfiguration.Standard.UsingFile("content.db").ShowSql(),
                    new DefaultPersistenceModelProvider(new DefaultAutoMappingProvider(), new AppDomainAssemblySelector())));
            repository.GenerateDatabaseSchema();
            GenerateContent();
        }

        public static IRepository Instance() {
            return repository;
        }

        static void GenerateContent() {
            using (var session = repository.OpenSession()) {
                HomePage parent = new HomePage { Title = "Parent", UrlSegment = "parent" };
                Page child = new Page { Title = "Child", UrlSegment = "child" };
                Page grandchild = new Page { Title = "Grand Child", UrlSegment = "grand-child" };
                parent.AddChildren(child);
                child.AddChildren(grandchild);

                Page child2 = new Page { Title = "Child 2", UrlSegment = "child2" };
                Page grandchild2 = new Page { Title = "Grand Child 2", UrlSegment = "grand-child" };
                parent.AddChildren(child2);
                child2.AddChildren(grandchild2);

                session.Save(parent);

                session.Flush();
            }
        }
    }
}
