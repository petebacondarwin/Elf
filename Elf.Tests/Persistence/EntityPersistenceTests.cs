using System.Collections.Generic;
using System.Linq;
using Elf.Persistence.Entities;
using NHibernate;
using NHibernate.Criterion;
using Ninject;
using Xunit;

namespace Elf.Tests.Persistence {
    public class EntityPersistenceTests {
        [Fact]
        public void TestNavigatingParentChildRelationship() {
            using (var kernel = TestHelper.CreateKernel()) {
                using (var session = kernel.Get<ISession>()) {
                    DatabaseHelper.GenerateDatabase(session);

                    var query = session.CreateCriteria<Page>().Add(Restrictions.Eq("UrlSegment", "grand-child"));
                    IList<Page> pages = query.List<Page>();

                    Assert.Equal(2, pages.Count);
                    Assert.True(pages.All(p => p.UrlSegment == "grand-child"));

                    query.CreateCriteria("Parent").Add(Restrictions.Eq("UrlSegment", "child"));
                    pages = query.List<Page>();

                    Assert.Equal(1, pages.Count);
                    Assert.True(pages.All(p => p.UrlSegment == "grand-child"));
                }
            }
        }

        [Fact]
        public void TestRetrieveByContentItem() {
            using (var kernel = TestHelper.CreateKernel()) {
                using (var session = kernel.Get<ISession>()) {
                    DatabaseHelper.GenerateDatabase(session);

                    Page page = new Page { Title = "Page 1", BodyText = "Body 1", UrlSegment = "page-1" };
                    HomePage home = new HomePage { Title = "Home", BodyText = "Home Body", UrlSegment = "~" };
                    home.AddChildren(page);
                    session.Save(home);

                    var contentItems = session.CreateCriteria<ContentItem>().List();
                    Assert.Equal(7, contentItems.Count);
                }
            }
        }
    }
}

