using AspNetCoreHero.ToastNotification.Abstractions;
using Martina.API.Data;
using Martina.API.Data.Entities;
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

      
        private readonly IBlobHelper _blobHelper;

        //private readonly ICombosHelper _combosHelper;

        //private readonly IMailHelper _mailHelper;

        public AccountController(IUserHelper userHelper, DataContext context, IBlobHelper blobHelper)
        {
            _userHelper = userHelper;
            _context = context;
            _blobHelper = blobHelper;


            //_combosHelper = combosHelper;

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

        //        _notfy.Error("Email o contraseña incorrectos.");
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


        [HttpPost]
        public async Task<JsonResult> Register(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _userHelper.AddUserAsync(model, imageId);

                if (user == null)
                {
                    //ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado por otro usuario.");
                    //return View(model);
                    return Json("Email repeat");
                }

                //string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                //string tokenLink = Url.Action("ConfirmEmail", "Account", new
                //{
                //    userid = user.Id,
                //    token = myToken
                //}, protocol: HttpContext.Request.Scheme);

                //Response response = _mailHelper.SendMail(model.Username, "Vehicles - Confirmación de cuenta", $"<h1>Vehicles - Confirmación de cuenta</h1>" +
                //    $"Para habilitar el usuario, " +
                //    $"por favor hacer clic en el siguiente enlace: </br></br><a href = \"{tokenLink}\">Confirmar Email</a>");

                //if (response.IsSuccess)
                //{
                //    ViewBag.Message = "Las instrucciones para habilitar su cuenta han sido enviadas al correo.";
                //    return View(model);
                //}

                //ModelState.AddModelError(string.Empty, response.Message);

                return Json("Success");
            }

            //model.DocumentTypes = _combosHelper.GetComboDocumentTypes();
            return Json("Model invalid");
        }

        public async Task<IActionResult> ChangeUser()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            UserViewModel model = new()
            {
                Address = user.Address,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ImageId = user.ImageId,
                Id = user.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _userHelper.GetUserAsync(User.Identity.Name);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.ImageId = imageId;
                 await _userHelper.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");
            }

            //model.DocumentTypes = _combosHelper.GetComboDocumentTypes();
            return View(model);
        }

    }
}
