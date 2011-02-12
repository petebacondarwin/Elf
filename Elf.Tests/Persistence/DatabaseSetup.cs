using FluentNHibernate.Cfg.Db;
using Ninject;
using Elf.Persistence;
using NHibernate;

namespace Elf.Tests.Persistence {
    public static class DatabaseHelper {
        public static void GenerateDatabase(ISession session) {
            using (var kernel = TestHelper.CreateKernel()) {
                kernel.Get<SchemaManager>().GenerateDatabaseSchema(session);
                HomePage parent = new HomePage { Title = "Parent", UrlSegment = "~" };
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
