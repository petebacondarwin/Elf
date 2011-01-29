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
            using (IRepository repository = TestRepository.Instance()) {
                using (var session = repository.OpenSession()) {
                    var query = session.CreateCriteria<Page>().Add(Expression.Eq("UrlSegment", "grand-child"));
                    IList<Page> pages = query.List<Page>();

                    Assert.That(pages, Has.Count.EqualTo(2));
                    Assert.That(pages.All(p => p.UrlSegment == "grand-child"));

                    query.CreateCriteria("Parent").Add(Expression.Eq("UrlSegment", "child"));
                    pages = query.List<Page>();

                    Assert.That(pages, Has.Count.EqualTo(1));
                    Assert.That(pages.All(p => p.UrlSegment == "grand-child"));
                }
            }
        }

        [Test]
        public void TestRetrieveByContentItem() {
            using (IRepository repository = TestRepository.Instance()) {
                using (var session = repository.OpenSession()) {
                    using (var transaction = session.BeginTransaction()) {
                        Page page = new Page { Title = "Page 1", BodyText = "Body 1", UrlSegment = "page-1" };
                        HomePage home = new HomePage { Title = "Home", BodyText = "Home Body", UrlSegment = "~" };
                        home.AddChildren(page);
                        session.Save(home);
                        transaction.Commit();
                    }
                }
                using (var session = repository.OpenSession()) {
                    var contentItems = session.CreateCriteria<ContentItem>().List();
                    Assert.That(contentItems.Count, Is.GreaterThan(0));
                }
            }
        }


    }
}

