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
    public class ApplicantController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Applicant/
        public ActionResult Index()
        {
            var applicants = db.Applicants.Include(a => a.Application).Include(a => a.EnglishProficiency);
            return View(applicants.ToList());
        }

        // GET: /Applicant/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Applicant applicant = db.Applicants.Find(id);
            if (applicant == null)
            {
                return HttpNotFound();
            }
            return View(applicant);
        }

        // GET: /Applicant/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Applicant/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,FirstName,MiddleName,LastName,Gender,MarritalStatus,DateOfBirth,CountryOfCitizen,CountryOfBirth,CurrentCity,VisaStatus,Skype,PhoneHome,PhoneWork,IsAppliedBefore")] Applicant applicant)
        {
            if (ModelState.IsValid)
            {
                db.Applicants.Add(applicant);
                db.SaveChanges();

                ////Create Application Starting informaiton with status
                //Application application = new Application();
                //application.Email = User.Identity.Name;
                //application.StartDate = DateTime.Now;
                //application.IsSubmitted = false;
                
                //db.Applications.Add(application);
                //db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(applicant);
        }

        // GET: /Applicant/Edit
        public ActionResult Edit()
        {
            Applicant applicant = db.Applicants.Find(User.Identity.Name);
            if (applicant == null)
            {
                return HttpNotFound();
            }
            return View(applicant);
        }

        // POST: /Applicant/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Email,FirstName,MiddleName,LastName,MarritalStatus,DateOfBirth,CountryOfCitizen,CountryOfBirth,CurrentCity,VisaStatus,Skype,PhoneHome,PhoneWork,IsAppliedBefore")] Applicant applicant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicant);
        }

        // GET: /Applicant/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Applicant applicant = db.Applicants.Find(id);
            if (applicant == null)
            {
                return HttpNotFound();
            }
            return View(applicant);
        }

        // POST: /Applicant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Applicant applicant = db.Applicants.Find(id);
            db.Applicants.Remove(applicant);
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
