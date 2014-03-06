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
        public ActionResult Index()
        {
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