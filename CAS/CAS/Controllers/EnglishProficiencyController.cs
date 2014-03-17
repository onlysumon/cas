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
    public class EnglishProficiencyController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /EnglishProficiency/
        public ActionResult Index()
        {
            var englishproficiencies = db.EnglishProficiencies.Include(e => e.Applicant).Include(e => e.Gre).Include(e => e.Ielts).Include(e => e.Toefl);
            return View(englishproficiencies.ToList());
        }

        // GET: /EnglishProficiency/Information
        public ActionResult Information()
        {
            if (db.EnglishProficiencies.Find(User.Identity.Name) == null)
                return RedirectToAction("Create");
            return RedirectToAction("Edit");
        }

        // GET: /EnglishProficiency/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnglishProficiency englishproficiency = db.EnglishProficiencies.Find(id);
            if (englishproficiency == null)
            {
                return HttpNotFound();
            }
            return View(englishproficiency);
        }

        // GET: /EnglishProficiency/Create
        public ActionResult Create()
        {
            //ViewBag.Email = new SelectList(db.Applicants, "Email", "LastName");
            return View();
        }

        // POST: /EnglishProficiency/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,Reading,Writting,Listening,Speaking,Ielts,Toefl,Gre")] EnglishProficiency englishproficiency)
        {
            if (ModelState.IsValid)
            {
                db.EnglishProficiencies.Add(englishproficiency);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(englishproficiency);
        }

        // GET: /EnglishProficiency/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnglishProficiency englishproficiency = db.EnglishProficiencies.Find(id);
            if (englishproficiency == null)
            {
                return HttpNotFound();
            }
            ViewBag.Email = new SelectList(db.Applicants, "Email", "FirstName", englishproficiency.Email);
            ViewBag.Email = new SelectList(db.GREs, "Email", "Email", englishproficiency.Email);
            ViewBag.Email = new SelectList(db.IELTS, "Email", "Email", englishproficiency.Email);
            ViewBag.Email = new SelectList(db.TOEFLs, "Email", "Email", englishproficiency.Email);
            return View(englishproficiency);
        }

        // POST: /EnglishProficiency/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Email,Reading,Writting,Listening,Speaking")] EnglishProficiency englishproficiency)
        {
            if (ModelState.IsValid)
            {
                db.Entry(englishproficiency).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Email = new SelectList(db.Applicants, "Email", "FirstName", englishproficiency.Email);
            ViewBag.Email = new SelectList(db.GREs, "Email", "Email", englishproficiency.Email);
            ViewBag.Email = new SelectList(db.IELTS, "Email", "Email", englishproficiency.Email);
            ViewBag.Email = new SelectList(db.TOEFLs, "Email", "Email", englishproficiency.Email);
            return View(englishproficiency);
        }

        // GET: /EnglishProficiency/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnglishProficiency englishproficiency = db.EnglishProficiencies.Find(id);
            if (englishproficiency == null)
            {
                return HttpNotFound();
            }
            return View(englishproficiency);
        }

        // POST: /EnglishProficiency/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            EnglishProficiency englishproficiency = db.EnglishProficiencies.Find(id);
            db.EnglishProficiencies.Remove(englishproficiency);
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
