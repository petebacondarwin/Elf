using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Elf.Persistence.Entities;

namespace Elf.Web.Mvc.TestSite.Areas.Main.Models {
    public class Page : ContentItem {
        [Required]
        public virtual string Title { get; set; }

        [Display(Name="Navigation Title")]
        public virtual string NavigationTitle { get; set; }

        [Display(Name="Body Text")]
        public virtual string BodyText { get; set; }
    }
}