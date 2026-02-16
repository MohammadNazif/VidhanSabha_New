using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlobalVidhanSabha.Controllers
{
    public class MasterController : Controller
    {
        // GET: Master

        public ActionResult MasterDashboard()
        {
            return View();
        }
        public ActionResult addDesignation()
        {
            return View();

        }
        public ActionResult addStateCount()
        {
            return View();
        }
        public ActionResult addDistrictCount()
        {
            return View();
        }
        public ActionResult VidhanSabhaRagiter()
        {
            return View();
        }

    }
}