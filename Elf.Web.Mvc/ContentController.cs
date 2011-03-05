using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Elf.Persistence.Entities;
using Elf.Web.Mvc.Utils;
using System.Reflection;

namespace Elf.Web.Mvc {
    public class ContentController<T> : Controller where T : ContentItem {
        /// <summary>
        /// The content item for which this object is controller.
        /// </summary>
        T CurrentItem {
            get {
                return (T) ControllerContext.RouteData.Values.GetContentItem();
            }
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext) {
            filterContext.RouteData.DataTokens["area"] = GetAreaFromNamespace();
            base.OnResultExecuting(filterContext);
        }

        public virtual string GetAreaFromNamespace() {
            string area = "";
            string controllerNamespace = this.GetType().Namespace;
            var match = System.Text.RegularExpressions.Regex.Match(controllerNamespace, @".*\.Areas\.(?<area>.*)\.Controllers");
            if (match.Success) {
                area = match.Groups["area"].Value;
            }
            return area;
        }
    }
}
