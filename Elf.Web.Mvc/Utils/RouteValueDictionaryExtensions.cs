using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using Elf.Persistence.Entities;

namespace Elf.Web.Mvc.Utils {
    public static class RouteValueDictionaryExtensions {
        public static void SetContentItem(this RouteValueDictionary dict, ContentItem item) {
            dict["content-item"] = item;
        }

        public static ContentItem GetContentItem(this RouteValueDictionary dict) {
            return dict["content-item"] as ContentItem;
        }
    }
}
