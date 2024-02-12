using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assignment_CT.Models;

namespace Assignment_CT.Controllers
{
    public class ClinicsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Clinics
        public ActionResult Index()
        {
            return View(db.Clinics.ToList());
        }

        // GET: Clinics/GetClinics
        public JsonResult GetClinicsList()
        {
            var clinics = db.Clinics.ToList();
            return new JsonResult
            {
                Data = clinics,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        // GET: Clinics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinics clinics = db.Clinics.Find(id);
            if (clinics == null)
            {
                return HttpNotFound();
            }
            return View(clinics);
        }

        // GET: Clinics/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clinics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClinicId,ClinicName,ClinicAddress")] Clinics clinics)
        {
            if (ModelState.IsValid)
            {
                db.Clinics.Add(clinics);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clinics);
        }

        // GET: Clinics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinics clinics = db.Clinics.Find(id);
            if (clinics == null)
            {
                return HttpNotFound();
            }
            return View(clinics);
        }

        // POST: Clinics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClinicId,ClinicName,ClinicAddress")] Clinics clinics)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clinics).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clinics);
        }

        // GET: Clinics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinics clinics = db.Clinics.Find(id);
            if (clinics == null)
            {
                return HttpNotFound();
            }
            return View(clinics);
        }

        // POST: Clinics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clinics clinics = db.Clinics.Find(id);
            db.Clinics.Remove(clinics);
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
