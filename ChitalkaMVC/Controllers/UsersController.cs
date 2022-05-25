using ChitalkaMVC.Logic.Users;
using ChitalkaMVC.Models;
using ChitalkaMVC.Storage.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChitalkaMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserManager _manager;
        public UsersController(IUserManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var au = HttpContext.Session.GetInt32("_Auth");
            if (au == 1)
                return RedirectToAction(nameof(Logout));
            return View(new UserViewModel { Result = " " });
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Username,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var authRes = (auth: false, admin: false);
                authRes = await _manager.Find(user);
                if (authRes.auth)
                {
                    HttpContext.Session.SetString("_Username", user.Username);
                    HttpContext.Session.SetInt32("_Auth", 1);
                    if (authRes.admin)
                        HttpContext.Session.SetInt32("_IsAdmin", 1);
                    else
                        HttpContext.Session.SetInt32("_IsAdmin", 0);
                    return RedirectToAction(nameof(Profile));
                }
                return View(new UserViewModel { Result = "Incorrect password or user does not exist", User = user });
            }
            return View(new UserViewModel { Result = "Check your input", User = user });
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var au = HttpContext.Session.GetInt32("_Auth");
            if (au != 1)
                return RedirectToAction("Login");
            ViewBag.isadmin = HttpContext.Session.GetInt32("_IsAdmin");
            ViewBag.auth = au;
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            if (HttpContext.Session.GetInt32("_Auth") != 1)
                return RedirectToAction("Login");
            var user = await _manager.Get(HttpContext.Session.GetString("_Username"));
            return View(user);
        }
        [HttpGet]
        public IActionResult Mail()
        {
            if (HttpContext.Session.GetInt32("_Auth") != 1)
                return RedirectToAction("Login");
            return View(new EmailConfirmationViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Mail(EmailConfirmationViewModel model)
        {
            if (HttpContext.Session.GetInt32("_Auth") != 1)
                return RedirectToAction("Login");
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetInt32("_VerCode") == null)
                {
                    Random rand = new Random();
                    var code = rand.Next(10000, 99999);
                    HttpContext.Session.SetInt32("_VerCode", code);
                    HttpContext.Session.SetString("_Mail", model.Email);
                    EMailService.SendEmail(model.Email, code);
                    model.Result = "Email Sent";
                }
                else
                {
                    if (model.ConfirmationCode != HttpContext.Session.GetInt32("_VerCode"))
                        model.Result = "Code is not valid";
                    else
                    {
                        var user = await _manager.Get(HttpContext.Session.GetString("_Username"));
                        user.Mail = HttpContext.Session.GetString("_Mail");
                        if (await _manager.Update(user))
                        HttpContext.Session.Remove("_VerCode");
                        HttpContext.Session.Remove("_Mail");
                        return RedirectToAction(nameof(Profile));
                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        [Route("users/logout")]
        public IActionResult LogoutConfirmed()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new UserViewModel { Result = " "});
        }

        [HttpPost]
        public async Task<IActionResult> Register([Bind("Username,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                if (await _manager.Create(user))
                {
                    return RedirectToAction(nameof(Login));
                    //return View(new UserViewModel { Result = "Success!", User = user });
                }
                return View(new UserViewModel { Result = "User already exists", User = user });
            }
            return View(new UserViewModel { Result = "Check your input", User = user });
        }
    }

}
