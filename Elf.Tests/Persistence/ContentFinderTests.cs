using Elf.Persistence;
using Elf.Tests.Entities;
using NUnit.Framework;
using NHibernate;
using Ninject;

namespace Elf.Tests.Persistence {
    [TestFixture]
    public class ContentFinderTests {
        [Test]
        public void TestFind() {
            using (var kernel = new StandardKernel(new TestModule())) {
                IContentFinder contentFinder = kernel.Get<IContentFinder>();

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
            using (var kernel = new StandardKernel(new TestModule())) {
                using (var session = kernel.Get<ISession>()) {
                    using (var transaction = session.BeginTransaction()) {
                        // Create a duplicate url (parent/child)
                        HomePage home2 = new HomePage() { UrlSegment = "parent" };
                        Page child2 = new Page() { UrlSegment = "child" };
                        home2.AddChildren(child2);
                        session.Save(home2);

                        IContentFinder contentFinder = new ContentFinder(session);
                        Page page = contentFinder.Find<Page>("parent/child");
                    }
                }
            }
        }
    }
}
