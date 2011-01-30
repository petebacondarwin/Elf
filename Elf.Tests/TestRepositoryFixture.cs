using Elf.Tests.Entities;
using Elf.Persistence;
using Elf.Persistence.Configuration;
using NUnit.Framework;
using Elf.Persistence.Configuration.AssemblySelectors;
using Elf.Persistence.Configuration.AutoMapping;
using System;

namespace Elf.Tests {
    [SetUpFixture]
    public class TestRepositoryFixture {
        [ThreadStatic]
        static IRepository repository;
            
        [SetUp]
        public void Create() {
            repository = new Repository(
                new RepositoryConfigurationProvider(
                    FluentNHibernate.Cfg.Db.SQLiteConfiguration
                        .Standard
                        .UsingFile("content.db")
                        .ShowSql()
                        .CurrentSessionContext<NHibernate.Context.ThreadStaticSessionContext>(),
                new DefaultPersistenceModelProvider(new DefaultAutoMappingProvider(), new AppDomainAssemblySelector())));
            repository.GenerateDatabaseSchema();
            // We have to start our own "current session" - normally this would be done in BeginRequest for ASP.NET
            ((Repository)repository).BeginSession();
            GenerateContent();
        }

        [TearDown]
        public void Finish() {
            // We also have to end the session ourselves to commit any transactions - this would be done in EndRequest for ASP.NET
            ((Repository)repository).EndSession();
        }

        public static IRepository Instance() {
            return repository;
        }

        static void GenerateContent() {
            var session = repository.CurrentSession;
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
        }
    }
}
