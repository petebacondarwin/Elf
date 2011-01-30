using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elf.Persistence;
using NHibernate;
using NHibernate.Criterion;
using Elf.Entities;

namespace Elf.Persistence {
    /// <summary>
    /// Describes the service that will find a content item in the repository via its url
    /// </summary>
    public interface IContentFinder {
        /// <summary>
        /// Find a content item from its url
        /// </summary>
        ContentItem Find(string url);
        TContentItem Find<TContentItem>(string url) where TContentItem : ContentItem;
    }

    /// <summary>
    /// Searches the repository to find a content item from its url
    /// </summary>
    public class ContentFinder : IContentFinder {
        readonly IRepository repository;

        /// <summary>
        /// Create a ContentFinder that looks in the specified repository.
        /// </summary>
        /// <param name="repository">The repository in which to search.</param>
        public ContentFinder(IRepository repository) {
            this.repository = repository;
        }

        public virtual ContentItem Find(string url) {
            IList<string> urlSegments = url.Split('/').Reverse().ToList();
            var session = repository.CurrentSession;
            if (urlSegments.Count > 0) {
                ICriteria criteria = session.CreateCriteria<ContentItem>();
                AddUrlSegmentExpression(criteria, urlSegments.First());
                foreach (string urlSegment in urlSegments.Skip(1)) {
                    criteria = criteria.CreateCriteria("Parent");
                    AddUrlSegmentExpression(criteria, urlSegment);
                }
                criteria.Add(Expression.IsNull("Parent"));
                try {
                    return criteria.UniqueResult<ContentItem>();
                } catch (NonUniqueResultException x) {
                    throw new Exception("The url provided does not uniquely identify a Content Item in the repository", x);
                }
            } else {
                return null;
            }
        }

        public virtual TContentItem Find<TContentItem>(string url) where TContentItem : ContentItem {
            return Find(url) as TContentItem;
        }

        protected virtual ICriteria AddUrlSegmentExpression(ICriteria criteria, string urlSegment) {
            return criteria.Add(Expression.Eq("UrlSegment", urlSegment));
        }
    }
}
