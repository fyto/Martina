using AspNetCoreHero.ToastNotification.Abstractions;
using Martina.API.Data;
using Martina.API.Helpers;
using Martina.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;

        private readonly INotyfService _notfy;

        //private readonly ICombosHelper _combosHelper;
        //private readonly IBlobHelper _blobHelper;
        //private readonly IMailHelper _mailHelper;

        public AccountController(IUserHelper userHelper, DataContext context, INotyfService notfy)
        {
            _userHelper = userHelper;
            _context = context;

            _notfy = notfy;
            //_combosHelper = combosHelper;
            //_blobHelper = blobHelper;
            //this._mailHelper = mailHelper;
        }


        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            return View(new LoginViewModel());
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
        //        if (result.Succeeded)
        //        {
        //            if (Request.Query.Keys.Contains("ReturnUrl"))
        //            {
        //                _notfy.Success("Inicio de sesión exitoso.");
        //                return Redirect(Request.Query["ReturnUrl"].First());
        //            }

        //            _notfy.Success("Inicio de sesión exitoso.");
        //            return RedirectToAction("Index", "Home");
        //        }

        //         _notfy.Error("Email o contraseña incorrectos.");
        //        //ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
        //    }
        //    return View(model);
        //}


        [HttpPost]
        public async Task<JsonResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        var returnUrl = Request.Query["ReturnUrl"].First();

                        return Json("Hola cabros");
                    }
                    
                    //return RedirectToAction("Index", "Home");
                    return Json("Index/Home");
                }

                return Json("Email o contraseña incorrectos");
                //ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
            }
            return Json("Model invalid");
            //return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }



    }
}
