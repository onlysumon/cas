using CAS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CAS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (db.Applicants.Find(User.Identity.Name) == null)
                ViewBag.IsNew = true;
            else
                ViewBag.IsNew = false;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Compro Application System.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Compro Application System.";

            return View();
        }
    }
}