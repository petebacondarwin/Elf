using Elf.Tests.Entities;
using FluentNHibernate.Cfg.Db;
using Ninject;
using Elf.Persistence;
using NHibernate;

namespace Elf.Tests.Persistence {
    public class TestModule : Elf.Persistence.Modules.CoreModule {
        public override IPersistenceConfigurer GetDatabaseConfiguration(Ninject.Activation.IContext context) {
            return SQLiteConfiguration.Standard.UsingFile("content.db").ShowSql();
        }
    }

    public static class DatabaseHelper {
        public static void GenerateDatabase() {
            var kernel = new StandardKernel(new TestModule());
            kernel.Get<SchemaManagement>().GenerateDatabaseSchema();
            using (var session = kernel.Get<ISession>()) {
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
