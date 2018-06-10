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
    public class Staff_DomainsController : Controller
    {
        private ProjectFinalHorus20180610020451_dbEntities db = new ProjectFinalHorus20180610020451_dbEntities();

        // GET: Staff_Domains
        public ActionResult Index()
        {
            var staff_Domains = db.Staff_Domains.Include(s => s.Domain).Include(s => s.Staff);
            return View(staff_Domains.ToList());
        }

        // GET: Staff_Domains/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff_Domains staff_Domains = db.Staff_Domains.Find(id);
            if (staff_Domains == null)
            {
                return HttpNotFound();
            }
            return View(staff_Domains);
        }

        // GET: Staff_Domains/Create
        public ActionResult Create()
        {
            ViewBag.id_domain = new SelectList(db.Domains, "id", "name");
            ViewBag.id_staff = new SelectList(db.Staffs, "id", "role");
            return View();
        }

        // POST: Staff_Domains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_staff,id_domain,active")] Staff_Domains staff_Domains)
        {
            if (ModelState.IsValid)
            {
                db.Staff_Domains.Add(staff_Domains);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_domain = new SelectList(db.Domains, "id", "name", staff_Domains.id_domain);
            ViewBag.id_staff = new SelectList(db.Staffs, "id", "role", staff_Domains.id_staff);
            return View(staff_Domains);
        }

        // GET: Staff_Domains/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff_Domains staff_Domains = db.Staff_Domains.Find(id);
            if (staff_Domains == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_domain = new SelectList(db.Domains, "id", "name", staff_Domains.id_domain);
            ViewBag.id_staff = new SelectList(db.Staffs, "id", "role", staff_Domains.id_staff);
            return View(staff_Domains);
        }

        // POST: Staff_Domains/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_staff,id_domain,active")] Staff_Domains staff_Domains)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staff_Domains).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_domain = new SelectList(db.Domains, "id", "name", staff_Domains.id_domain);
            ViewBag.id_staff = new SelectList(db.Staffs, "id", "role", staff_Domains.id_staff);
            return View(staff_Domains);
        }

        // GET: Staff_Domains/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff_Domains staff_Domains = db.Staff_Domains.Find(id);
            if (staff_Domains == null)
            {
                return HttpNotFound();
            }
            return View(staff_Domains);
        }

        // POST: Staff_Domains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Staff_Domains staff_Domains = db.Staff_Domains.Find(id);
            db.Staff_Domains.Remove(staff_Domains);
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
