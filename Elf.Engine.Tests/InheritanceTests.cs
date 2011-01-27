using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Elf.Engine.Tests.Entities;
using Elf.Persistence;

namespace Elf.Engine.Tests {
    [TestFixture]
    public class InheritanceTests {
        [Test]
        public void TestRetrieveByContentItem() {
            IRepository repository = CreateRepository();

            using (var session = repository.OpenSession()) {
                // populate the database
                using (var transaction = session.BeginTransaction()) {
                    Page page1 = new Page { Title = "Page 1", BodyText = "Body 1", UrlSegment = "page-1" };
                    Page page2 = new Page { Title = "Page 2", BodyText = "Body 2", UrlSegment = "page-2" };
                    Page page3 = new Page { Title = "Page 3", BodyText = "Body 3", UrlSegment = "page-3" };
                    Page page4 = new Page { Title = "Page 4", BodyText = "Body 4", UrlSegment = "page-4" };
                    HomePage home = new HomePage { Title = "Home", BodyText = "Home Body", UrlSegment = "~" };
                    home.AddChildren(page1, page2, page3, page4);
                    session.SaveOrUpdate(home);
                    transaction.Commit();
                }
            }
            using (var session = repository.OpenSession()) {
                var contentItems = session.CreateCriteria(typeof(Elf.Entities.ContentItem)).List<Elf.Entities.ContentItem>();
                Assert.That(contentItems.Count, Is.GreaterThan(0));
            }
        }

        IRepository CreateRepository() {
            return new Repository(
                new RepositoryConfiguration(
                    FluentNHibernate.Cfg.Db.SQLiteConfiguration.Standard.UsingFile("content.db"),
                    new AutomappingConfiguration(),
                    new AppDomainAssemblySelector()));

        }
    }
}

