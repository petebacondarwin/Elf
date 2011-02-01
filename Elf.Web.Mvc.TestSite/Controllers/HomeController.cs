using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;

namespace Elf.Web.Mvc.TestSite.Controllers {
    public class HomeController : Controller {
        readonly ISession session;
        public HomeController(ISession session) {
            this.session = session;
        }

        public ActionResult Index() {
            ViewBag.Message = session.SessionFactory.GetAllClassMetadata().Keys.Aggregate((a,b)=>a+b);

            return View();
        }

        public ActionResult About() {
            return View();
        }
    }
}
