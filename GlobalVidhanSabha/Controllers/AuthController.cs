using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VishanSabha.Models;
using VishanSabha.Services.Auth;

namespace VishanSabha.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController()
        {
            _authService = new AuthService();
        }

        // GET: Login Page
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Handle Login
        [HttpPost]
        public ActionResult Login(string Contact, string Password)
        {
            var user = _authService.ValidateUser(Contact, Password);


            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Contact, false);

                Session["Contact"] = user.Contact;
                Session["VidhanSabhaId"] = user.VidhanSabhaId;
                string contact = Session["Contact"].ToString();
                int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
                if(user.Role== "SuperAdmin")
                {
                    TempData["LoginSuccess"] = $"Welcome back, {user.Contact}!";
                    return RedirectToAction("MasterDashboard", "Master", new { loginSuccess = 1 });
                }
               else if (user.Role == "MLA")
                {
                    TempData["LoginSuccess"] = $"Welcome back, {user.Contact}!";
                    return RedirectToAction("Dashboard", "Admin", new { loginSuccess = 1 });
                }
                else if (user.Role == "SectorIncharge")
                {
                    TempData["LoginSuccess"] = $"Welcome back, {user.Contact}!";
                    return RedirectToAction("SectorDashboard", "Sector");
                    //, new{
                    //    contact = user.Contact,
                    //    Role = user.Role // or user.Name if exists
                    //});
                }
                else if (user.Role == "BoothIncharge")
                {
                    TempData["LoginSuccess"] = $"Welcome back, {user.Contact}!";
                    return RedirectToAction("BoothDashboard", "Booth", new { loginSuccess = 1 });
                }
                else
                {
                    TempData["LoginSuccess"] = $"Welcome back, {user.Contact}!";
                    return RedirectToAction("Dashboard", "Admin", new { loginSuccess = 1 });
                }

            }

            TempData["LoginError"] = "Invalid contact or password!";
            return View();
        }

        // Logout
        //[Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Auth");
        }
    }
}
