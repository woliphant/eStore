using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Http;
using eStore.Models;
using eStore.ViewModels;
using eStore.Utils;
using Microsoft.AspNet.Authorization;

namespace eStore.Controllers
{
    public class LoginController : Controller
    {
        UserManager<ApplicationUser> _usrMgr;
        SignInManager<ApplicationUser> _signInMgr;
        public LoginController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
        {
            _usrMgr = userManager;
            _signInMgr = signInManager;
        }

        // GET: Login
        [AllowAnonymous]
        public ActionResult Index(string ReturnUrl = null)
        {
            if (HttpContext.Session.Get(SessionVars.LoginStatus) == null)
            {
                HttpContext.Session.SetString(SessionVars.LoginStatus, "not logged in");
            }
            if (Convert.ToString(HttpContext.Session.Get(SessionVars.LoginStatus)) == "not logged in")
            {
                HttpContext.Session.SetString(SessionVars.Message, "most functionality requires you to login!");
            }
            ViewBag.Status = HttpContext.Session.GetString(SessionVars.LoginStatus);
            ViewBag.Message = HttpContext.Session.GetString(SessionVars.Message);
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect("/Home");
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInMgr.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    HttpContext.Session.SetString(SessionVars.User, model.Email);
                    HttpContext.Session.SetString(SessionVars.LoginStatus, "logged in as " + model.Email);
                    HttpContext.Session.SetString(SessionVars.Message, "Welcome " + model.Email);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    HttpContext.Session.SetString(SessionVars.Message, "login attempt failed");
                    ViewBag.Message = HttpContext.Session.GetString(SessionVars.Message);
                    return View("Index");
                }
            }
            return RedirectToLocal(returnUrl);
        }

        // Logoff Method
        public async Task<IActionResult> LogOff(string returnUrl = null)
        {
            await _signInMgr.SignOutAsync();
            HttpContext.Session.Clear();
            HttpContext.Session.SetString(SessionVars.LoginStatus, "not logged in");
            return Redirect("/Home");
        }
    }
}