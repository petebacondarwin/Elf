using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Elf.Tests.Entities;
using Elf.Persistence;
using NHibernate.Criterion;
using Elf.Entities;

namespace Elf.Tests {
    [TestFixture]
    public class RepositoryTests {
        [Test]
        public void TestNavigatingParentChildRelationship() {
            IRepository repository = TestRepositoryFixture.Instance();
            var session = repository.CurrentSession;
            var query = session.CreateCriteria<Page>().Add(Expression.Eq("UrlSegment", "grand-child"));
            IList<Page> pages = query.List<Page>();

            Assert.That(pages, Has.Count.EqualTo(2));
            Assert.That(pages.All(p => p.UrlSegment == "grand-child"));

            query.CreateCriteria("Parent").Add(Expression.Eq("UrlSegment", "child"));
            pages = query.List<Page>();

            Assert.That(pages, Has.Count.EqualTo(1));
            Assert.That(pages.All(p => p.UrlSegment == "grand-child"));
        }

        [Test]
        public void TestRetrieveByContentItem() {
            IRepository repository = TestRepositoryFixture.Instance();
            var session = repository.CurrentSession;
            Page page = new Page { Title = "Page 1", BodyText = "Body 1", UrlSegment = "page-1" };
            HomePage home = new HomePage { Title = "Home", BodyText = "Home Body", UrlSegment = "~" };
            home.AddChildren(page);
            session.Save(home);

            var contentItems = session.CreateCriteria<ContentItem>().List();
            Assert.That(contentItems.Count, Is.GreaterThan(0));
        }
    }
}

