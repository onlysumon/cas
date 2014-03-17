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
    public class EmploymentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Employment/
        public ActionResult Index()
        {
            return View(db.Employments.ToList());
        }

        // GET: /Employment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employment employment = db.Employments.Find(id);
            if (employment == null)
            {
                return HttpNotFound();
            }
            return View(employment);
        }

        // GET: /Employment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Employment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,ComnapyName,WebSite,City,Country,FromDate,ToDate,Position,EmployementType,Programming,DataStructur,Networking,DatabaseAdministration,Teaching,Management,Others,OthersPercentage")] Employment employment)
        {
            if (ModelState.IsValid)
            {
                db.Employments.Add(employment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employment);
        }

        // GET: /Employment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employment employment = db.Employments.Find(id);
            if (employment == null)
            {
                return HttpNotFound();
            }
            return View(employment);
        }

        // POST: /Employment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,ComnapyName,WebSite,City,Country,FromDate,ToDate,Position,EmployementType,Programming,DataStructur,Networking,DatabaseAdministration,Teaching,Management,Others,OthersPercentage")] Employment employment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employment);
        }

        // GET: /Employment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employment employment = db.Employments.Find(id);
            if (employment == null)
            {
                return HttpNotFound();
            }
            return View(employment);
        }

        // POST: /Employment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employment employment = db.Employments.Find(id);
            db.Employments.Remove(employment);
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
