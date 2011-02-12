using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Linq;
using Elf.Persistence.Entities;
using Elf.Web.Mvc.TestSite.Modules.Main.Models;
using Elf.Persistence;

namespace Elf.Web.Mvc.TestSite.Modules.Main.Controllers
{
    public class PageController : Controller
    {
        readonly ISession session;
        readonly SchemaManager schemaManager;

        public PageController(ISession session, SchemaManager schemaManager) {
            this.session = session;
            this.schemaManager = schemaManager;
        }

        public ActionResult CreateDatabase() {
            schemaManager.GenerateDatabaseSchema();
            return RedirectToAction("Index");
        }

        public ActionResult Index() {
            var contentItems = from item in session.Query<Page>() select item;
            return View(contentItems.ToList());
        }

        public ActionResult Details(int id)
        {
            return View(session.Get<Page>(id));
        }

        public ActionResult Create()
        {
            Page page = new Page();
            if (TryUpdateModel<Page>(page)) {
                session.Save(page);
                session.Flush();
                return View("Details", page);
            } else {
                ViewBag.updateError = "Failed to create page";
            }
            return View(page);
        }
        
        public ActionResult Edit(int id)
        {
            Page page = session.Get<Page>(id);
            if (Request.HttpMethod == "POST") {
                if (TryUpdateModel<Page>(page)) {
                    session.Update(page);
                    session.Flush();
                    return RedirectToAction("Index");
                }
            }
            return View(page);
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
