using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectFinalHorus.Models;

namespace ProjectFinalHorus.Controllers
{
    public class Clients_PackagesController : Controller
    {
        private ProjectFinalHorus20180610020451_dbEntities db = new ProjectFinalHorus20180610020451_dbEntities();

        // GET: Clients_Packages
        public ActionResult Index()
        {
            var clients_Packages = db.Clients_Packages.Include(c => c.Client).Include(c => c.Domain).Include(c => c.Package);
            return View(clients_Packages.ToList());
        }

        // GET: Clients_Packages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients_Packages clients_Packages = db.Clients_Packages.Find(id);
            if (clients_Packages == null)
            {
                return HttpNotFound();
            }
            return View(clients_Packages);
        }

        // GET: Clients_Packages/Create
        public ActionResult Create()
        {
            ViewBag.id_client = new SelectList(db.Clients, "id", "first_name");
            ViewBag.id_domain = new SelectList(db.Domains, "id", "name");
            ViewBag.id_package = new SelectList(db.Packages, "id", "name");
            return View();
        }

        // POST: Clients_Packages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_client,id_domain,id_package,payment_method,timestamp")] Clients_Packages clients_Packages)
        {
            if (ModelState.IsValid)
            {
                db.Clients_Packages.Add(clients_Packages);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_client = new SelectList(db.Clients, "id", "first_name", clients_Packages.id_client);
            ViewBag.id_domain = new SelectList(db.Domains, "id", "name", clients_Packages.id_domain);
            ViewBag.id_package = new SelectList(db.Packages, "id", "name", clients_Packages.id_package);
            return View(clients_Packages);
        }

        // GET: Clients_Packages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients_Packages clients_Packages = db.Clients_Packages.Find(id);
            if (clients_Packages == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_client = new SelectList(db.Clients, "id", "first_name", clients_Packages.id_client);
            ViewBag.id_domain = new SelectList(db.Domains, "id", "name", clients_Packages.id_domain);
            ViewBag.id_package = new SelectList(db.Packages, "id", "name", clients_Packages.id_package);
            return View(clients_Packages);
        }

        // POST: Clients_Packages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_client,id_domain,id_package,payment_method,timestamp")] Clients_Packages clients_Packages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clients_Packages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_client = new SelectList(db.Clients, "id", "first_name", clients_Packages.id_client);
            ViewBag.id_domain = new SelectList(db.Domains, "id", "name", clients_Packages.id_domain);
            ViewBag.id_package = new SelectList(db.Packages, "id", "name", clients_Packages.id_package);
            return View(clients_Packages);
        }

        // GET: Clients_Packages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients_Packages clients_Packages = db.Clients_Packages.Find(id);
            if (clients_Packages == null)
            {
                return HttpNotFound();
            }
            return View(clients_Packages);
        }

        // POST: Clients_Packages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clients_Packages clients_Packages = db.Clients_Packages.Find(id);
            db.Clients_Packages.Remove(clients_Packages);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
