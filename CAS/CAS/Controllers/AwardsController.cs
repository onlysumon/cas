using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CAS.Models;
using CAS.DAL;

namespace CAS.Controllers
{
    public class AwardsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Awards/
        public ActionResult Index()
        {
            return View(db.AwardsAndHonors.ToList());
        }

        // GET: /Awards/Honours
        public ActionResult Honours()
        {
            if (db.AwardsAndHonors.Find(User.Identity.Name) == null)
                return RedirectToAction("Create");
            return RedirectToAction("Edit");
        }

        // GET: /Awards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AwardsAndHonors awardsandhonors = db.AwardsAndHonors.Find(id);
            if (awardsandhonors == null)
            {
                return HttpNotFound();
            }
            return View(awardsandhonors);
        }

        // GET: /Awards/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Awards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Year,Description")] AwardsAndHonors awardsandhonors)
        {
            if (ModelState.IsValid)
            {
                db.AwardsAndHonors.Add(awardsandhonors);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(awardsandhonors);
        }

        // GET: /Awards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AwardsAndHonors awardsandhonors = db.AwardsAndHonors.Find(id);
            if (awardsandhonors == null)
            {
                return HttpNotFound();
            }
            return View(awardsandhonors);
        }

        // POST: /Awards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Year,Description")] AwardsAndHonors awardsandhonors)
        {
            if (ModelState.IsValid)
            {
                db.Entry(awardsandhonors).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(awardsandhonors);
        }

        // GET: /Awards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AwardsAndHonors awardsandhonors = db.AwardsAndHonors.Find(id);
            if (awardsandhonors == null)
            {
                return HttpNotFound();
            }
            return View(awardsandhonors);
        }

        // POST: /Awards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AwardsAndHonors awardsandhonors = db.AwardsAndHonors.Find(id);
            db.AwardsAndHonors.Remove(awardsandhonors);
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
