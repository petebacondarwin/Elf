using Elf.Persistence;
using Elf.Tests.Entities;
using NUnit.Framework;

namespace Elf.Tests {
    [TestFixture]
    public class ContentFinderTests {
        [Test]
        public void TestFind() {
            using (IRepository repository = TestRepository.Instance()) {
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
        }

        [Test, ExpectedException()]
        public void TestFindNonUniqueUrl() {
            using (IRepository repository = TestRepository.Instance()) {
                IContentFinder contentFinder = new ContentFinder(repository);

                Page page = contentFinder.Find<Page>("grand-child");
            }
        }
    }
}
