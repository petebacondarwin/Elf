using System;
using Elf.Entities;

namespace Elf.Tests.Entities {
    /// <summary>
    /// The base content item that will be stored in the database.
    /// </summary>
    public abstract class AbstractPage : ContentItem {
        public virtual string Title { get; set; }
        public AbstractPage()
            : base() {
        }
    }
    /// <summary>
    /// A basic page of content
    /// </summary>
    public class Page : AbstractPage {
        public virtual string BodyText { get; set; }
    }

    /// <summary>
    /// The homepage acts as the root of the content tree of pages
    /// </summary>
    public class HomePage : Page {
        public virtual string ExtraInfo { get; set; }
    }
}
