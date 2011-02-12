using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Elf.Persistence;
using NHibernate.Criterion;
using Elf.Persistence.Entities;
using Ninject;
using NHibernate;

namespace Elf.Tests.Persistence {
    [TestFixture]
    public class EntityPersistenceTests {
        [Test]
        public void TestNavigatingParentChildRelationship() {
            using (var kernel = TestHelper.CreateKernel()) {
                using (var session = kernel.Get<ISession>()) {
                    DatabaseHelper.GenerateDatabase(session);

                    var query = session.CreateCriteria<Page>().Add(Restrictions.Eq("UrlSegment", "grand-child"));
                    IList<Page> pages = query.List<Page>();

                    Assert.That(pages, Has.Count.EqualTo(2));
                    Assert.That(pages.All(p => p.UrlSegment == "grand-child"));

                    query.CreateCriteria("Parent").Add(Restrictions.Eq("UrlSegment", "child"));
                    pages = query.List<Page>();

                    Assert.That(pages, Has.Count.EqualTo(1));
                    Assert.That(pages.All(p => p.UrlSegment == "grand-child"));
                }
            }
        }

        [Test]
        public void TestRetrieveByContentItem() {
            using (var kernel = TestHelper.CreateKernel()) {
                using (var session = kernel.Get<ISession>()) {
                    DatabaseHelper.GenerateDatabase(session);

                    Page page = new Page { Title = "Page 1", BodyText = "Body 1", UrlSegment = "page-1" };
                    HomePage home = new HomePage { Title = "Home", BodyText = "Home Body", UrlSegment = "~" };
                    home.AddChildren(page);
                    session.Save(home);

                    var contentItems = session.CreateCriteria<ContentItem>().List();
                    Assert.That(contentItems.Count, Is.GreaterThan(0));
                }
            }
        }
    }
}

