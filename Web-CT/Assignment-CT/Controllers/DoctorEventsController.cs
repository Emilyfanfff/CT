using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assignment_CT.Models;
using Microsoft.AspNet.Identity;

namespace Assignment_CT.Controllers
{
    public class DoctorEventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DoctorEvents
        [Authorize(Roles = "Doctor")]
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId(); // Using ASP.NET Identity to get the user ID
            var DoctorEvent = db.DoctorEvents.Where(b => b.DoctorId == currentUserId).ToList();
            return View(DoctorEvent);
        }

        // GET: DoctorEvents/Details/5
        [Authorize(Roles = "Doctor")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorEvents doctorEvents = db.DoctorEvents.Include(d => d.Doctor).SingleOrDefault(d => d.EventId == id);
            if (doctorEvents == null)
            {
                return HttpNotFound();
            }
            return View(doctorEvents);
        }

        // GET: DoctorEvents/Create
        [Authorize(Roles = "Doctor")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: DoctorEvents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Doctor")]
        public ActionResult Create([Bind(Include = "EventId,StartTime,EndTime")] DoctorEvents doctorEvents)
        {
            if (ModelState.IsValid)
            {
                string doctorId = User.Identity.GetUserId();
                doctorEvents.DoctorId = doctorId;
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                if (user != null)
                {
                    doctorEvents.Doctor = user;
                }
                db.DoctorEvents.Add(doctorEvents);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(doctorEvents);
        }

        // GET: DoctorEvents/Edit/5
        [Authorize(Roles = "Doctor")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorEvents doctorEvents = db.DoctorEvents.Find(id);
            if (doctorEvents == null)
            {
                return HttpNotFound();
            }
            return View(doctorEvents);
        }

        // POST: DoctorEvents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Doctor")]
        public ActionResult Edit([Bind(Include = "EventId,StartTime,EndTime")] DoctorEvents doctorEvents)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctorEvents).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(doctorEvents);
        }

        // GET: DoctorEvents/Delete/5
        [Authorize(Roles = "Doctor")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorEvents doctorEvents = db.DoctorEvents.Include(d => d.Doctor).SingleOrDefault(d => d.EventId == id);
            if (doctorEvents == null)
            {
                return HttpNotFound();
            }
            return View(doctorEvents);
        }

        // POST: DoctorEvents/Delete/5
        [Authorize(Roles = "Doctor")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DoctorEvents doctorEvents = db.DoctorEvents.Find(id);
            db.DoctorEvents.Remove(doctorEvents);
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
