using System;
using System.Collections.Generic;
using Elf.Persistence;

namespace Elf.Entities {
    /// <summary>
    /// The base content item that will be stored in the database.
    /// </summary>
    public abstract class AbstractPage : ContentItem {
        public virtual string Title { get; set; }
        public AbstractPage() : base() {
        }
    }
}