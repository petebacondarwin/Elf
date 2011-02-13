using Elf.Persistence;
using NHibernate;
using Ninject;
using Xunit;

namespace Elf.Tests.Persistence {
    public class ContentFinderTests {
        [Fact]
        public void TestFind() {
            using (var kernel = TestHelper.CreateKernel()) {
                using (ISession session = kernel.Get<ISession>()) {

                    DatabaseHelper.GenerateDatabase(session);
                    IContentFinder contentFinder = new ContentFinder(session);

                    Page page = contentFinder.Find<Page>("~/child/grand-child");
                    Assert.NotNull(page);
                    Assert.Equal("Grand Child", page.Title);

                    page = contentFinder.Find<Page>("~");
                    Assert.NotNull(page);
                    Assert.Equal("Parent", page.Title);
                    Assert.Null(page.Parent);
                    Assert.IsType<HomePage>(page);

                    page = contentFinder.Find<Page>("~/child/other");
                    Assert.Null(page);
                }
            }
        }

        [Fact]
        public void NonUniqueUrlThrowsException() {
            using (var kernel = TestHelper.CreateKernel()) {
                using (var session = kernel.Get<ISession>()) {
                    DatabaseHelper.GenerateDatabase(session);
                    // Create a duplicate url (parent/child)
                    HomePage home2 = new HomePage() { UrlSegment = "~" };
                    Page child2 = new Page() { UrlSegment = "child" };
                    home2.AddChildren(child2);
                    session.Save(home2);

                    IContentFinder contentFinder = new ContentFinder(session);
                    Assert.Throws<System.Exception>(()=>
                        contentFinder.Find<Page>("~/child")
                    );
                }
            }
        }
    }
}
