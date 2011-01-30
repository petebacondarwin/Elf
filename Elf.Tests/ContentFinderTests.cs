using Elf.Persistence;
using Elf.Tests.Entities;
using NUnit.Framework;
using NHibernate;

namespace Elf.Tests {
    [TestFixture]
    public class ContentFinderTests {
        [Test]
        public void TestFind() {
            IRepository repository = TestRepositoryFixture.Instance();
            IContentFinder contentFinder = new ContentFinder(repository);

            Page page = contentFinder.Find<Page>("parent/child/grand-child");
            Assert.That(page, Is.Not.Null);
            Assert.That(page.Title, Is.EqualTo("Grand Child"));

            page = contentFinder.Find<Page>("parent");
            Assert.That(page, Is.Not.Null);
            Assert.That(page.Title, Is.EqualTo("Parent"));
            Assert.That(page.Parent, Is.Null);
            Assert.That(page, Is.InstanceOf<HomePage>());

            page = contentFinder.Find<Page>("parent/child/other");
            Assert.That(page, Is.Null);
        }

        [Test, ExpectedException()]
        public void TestFindNonUniqueUrl() {
            IRepository repository = TestRepositoryFixture.Instance();
            var session = repository.CurrentSession;
            // Create a duplicate url (parent/child)
            HomePage home2 = new HomePage() { UrlSegment = "parent" };
            Page child2 = new Page() { UrlSegment = "child" };
            home2.AddChildren(child2);
            session.Save(home2);

            IContentFinder contentFinder = new ContentFinder(repository);
            Page page = contentFinder.Find<Page>("parent/child");
        }
    }
}
