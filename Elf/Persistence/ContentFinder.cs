using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elf.Persistence;
using NHibernate;
using NHibernate.Criterion;
using Elf.Persistence.Entities;

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
        const string UrlSegmentFieldName = "UrlSegment";
        const string ParentFieldName = "Parent";

        readonly ISession session;

        /// <summary>
        /// Create an ContentFinder to work with the specified session
        /// </summary>
        public ContentFinder(ISession session) {
            this.session = session;
        }

        /// <summary>
        /// Find a content item from its url path.
        /// </summary>
        /// <remarks>
        /// The implementation builds a criteria based query by iterating over each segment
        /// of the given url and adding it as a "parent" sub-criterion.  The generated sql is a single query!
        /// The query insists that the top content item, matching the first urlsegment, has no parents.
        /// </remarks>
        /// <param name="url">The url with which to search for a content item</param>
        /// <returns>The content item with the given url or null if none exists</returns>
        /// <exception cref="System.Exception">If more than one content item matches the url</exception>
        public virtual ContentItem Find(string url) {
            IList<string> urlSegments = url.Split(new char[] {'/'}, StringSplitOptions.RemoveEmptyEntries).Reverse().ToList();
            if (urlSegments.Count > 0) {
                ICriteria criteria = session.CreateCriteria<ContentItem>();
                AddUrlSegmentExpression(criteria, urlSegments.First());
                foreach (string urlSegment in urlSegments.Skip(1)) {
                    criteria = criteria.CreateCriteria(ParentFieldName);
                    AddUrlSegmentExpression(criteria, urlSegment);
                }
                criteria.Add(Restrictions.IsNull(ParentFieldName));
                try {
                    return criteria.UniqueResult<ContentItem>();
                } catch (NonUniqueResultException x) {
                    throw new Exception("The url provided does not uniquely identify a Content Item in the repository", x);
                }
            } else {
                return null;
            }
        }

        /// <summary>
        /// Find a content item from its url path.
        /// </summary>
        /// <typeparam name="TContentItem">The type of the content item to return</typeparam>
        /// <param name="url">The url with which to search for a content item</param>
        /// <returns>The content item with the given url (cast to type <typeparamref name="TContentItem"/>) or null if none exists</returns>
        /// <exception cref="System.Exception">If more than one content item matches the url</exception>
        public virtual TContentItem Find<TContentItem>(string url) where TContentItem : ContentItem {
            return Find(url) as TContentItem;
        }

        protected virtual ICriteria AddUrlSegmentExpression(ICriteria criteria, string urlSegment) {
            return criteria.Add(Restrictions.Eq(UrlSegmentFieldName, urlSegment));
        }
    }
}
