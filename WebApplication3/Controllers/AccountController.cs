using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private ClinicManagementEntities dBEntities = new ClinicManagementEntities();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UsersViewModel userVM)
        {
            User model = new User();
            if (ModelState.IsValid)
            {
                model.UserName = userVM.Username;
                model.Password = userVM.Password;
                model.Email = userVM.Email;
               
                dBEntities.Users.Add(model);
                dBEntities.SaveChanges();
            }
            return RedirectToAction("Login");
        }
        public JsonResult IsUserNameAvailable(string UserName)
        {
            return Json(!dBEntities.Users.Any(u => u.UserName == UserName), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsEmailAvailable(string Email)
        {
            return Json(!dBEntities.Users.Any(u => u.Email == Email), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM model)
        {
            bool isValid = dBEntities.Users.Any(u => u.UserName == model.Username && u.Password == model.Password);
            if (isValid)
            {

                FormsAuthentication.SetAuthCookie(model.Username, false);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid Username/Password!");
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}