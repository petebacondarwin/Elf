﻿using System;
using System.Linq;
using System.Collections.Generic;
using Elf.Persistence;

namespace Elf.Persistence.Entities {
    /// <summary>
    /// The base content item that will be stored in the database.
    /// </summary>
    public class ContentItem : IPersistent {
        public virtual int Id { get; private set; }
        public virtual string UrlSegment { get; set; }
        public virtual IList<ContentItem> Children { get; private set; }
        public virtual ContentItem Parent { get; set; }

        public ContentItem() {
            Children = new List<ContentItem>();
        }

        public virtual void AddChildren(params ContentItem[] children) {
            foreach (ContentItem child in children) {
                Children.Add(child);
                child.Parent = this;
            }
        }
    }
}